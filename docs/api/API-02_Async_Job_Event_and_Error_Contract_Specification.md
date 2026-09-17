# **API-02 — Async Job, Event & Error Contract Specification**

**Document ID:** API-02  
**Document Type:** Async Job, Event & Error Contract Specification  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Primary Role:** API Designer / Backend Architect  
**Status:** Draft v1  
**Parent Documents:** ARCH-01, ARCH-02, API-01  
**Related Documents:** DATA-01, DATA-02, AI-01, AI-02, AI-03, QA-01

## **1. Purpose**

Tài liệu này định nghĩa contract cho các tác vụ bất đồng bộ của OWNIVERSE, đặc biệt là AI generation.

Nó trả lời:

> Khi user yêu cầu một tác vụ AI chạy lâu, frontend biết Job đang ở đâu, khi nào hoàn tất, lỗi gì xảy ra và lấy kết quả thế nào?

Tài liệu bao gồm:

- async request acknowledgement;

- Job lifecycle;

- polling contract;

- SignalR/realtime event;

- progress;

- cancellation;

- retry;

- regeneration boundary;

- idempotency;

- worker integration;

- error semantics;

- recovery;

- reconnect;

- stale/outdated notifications.

# **2. Core Principle**

Nguyên tắc trung tâm:

> **HTTP starts the work. Job state tracks the work. Realtime only announces the work.**

Nói đơn giản:

- HTTP tạo Job;

- Database giữ trạng thái Job;

- Worker thực hiện Job;

- SignalR chỉ giúp frontend biết nhanh rằng trạng thái đã thay đổi.

SignalR không phải source of truth.

# **3. JSend Convention**

Tất cả HTTP endpoint trong API-02 tiếp tục sử dụng JSend theo API-01:

2xx → success

4xx → fail

5xx → error

Ví dụ:

{

"status": "success",

"data": {}

}

Realtime event **không dùng JSend**, vì event không phải HTTP response.

# **4. Async Operation Types**

Các operation chính có thể chạy bất đồng bộ:

STORY_GENERATION

CHARACTER_GENERATION

SCENE_GENERATION

PROMPT_GENERATION

IMAGE_GENERATION

IMAGE_REGENERATION

CONSISTENCY_VALIDATION

BATCH_GENERATION

MVP ưu tiên:

IMAGE_GENERATION

IMAGE_REGENERATION

# **5. Generation Request**

Ví dụ:

POST /api/v1/scenes/{sceneId}/generations

Request:

{

"operationType": "IMAGE_GENERATION",

"candidateCount": 4,

"instruction": "Emphasize the rainy cinematic atmosphere."

}

# **6. Async Acknowledgement**

Backend không chờ AI hoàn tất.

Response:

202 Accepted

{

"status": "success",

"data": {

"job": {

"jobId": "33a4dd90-e69c-4bf6-a110-8798d43d9f19",

"operationType": "IMAGE_GENERATION",

"status": "QUEUED",

"target": {

"type": "SCENE",

"id": "scene-id"

},

"createdAt": "2026-09-13T15:50:00Z"

}

}

}

# **7. Why Return 202**

202 Accepted có nghĩa:

> Request hợp lệ và đã được chấp nhận để xử lý, nhưng chưa hoàn thành.

Không nên trả 200 như thể image đã tạo xong.

# **8. Job Resource**

Job là persistent resource.

Endpoint:

GET /api/v1/jobs/{jobId}

Frontend có thể query lại Job bất kỳ lúc nào, kể cả sau refresh browser.

# **9. Job Lifecycle**

Lifecycle chuẩn:

CREATED

↓

QUEUED

↓

CLAIMED

↓

PREPARING

↓

RUNNING

↓

VALIDATING

↓

COMPLETED

Nhánh phụ:

RETRY_PENDING

FAILED

REJECTED

CANCEL_REQUESTED

CANCELLED

# **10. User-Facing Status**

Không nhất thiết expose toàn bộ trạng thái internal cho UI.

Có thể map:

<table>
<colgroup>
<col style="width: 66%" />
<col style="width: 33%" />
</colgroup>
<thead>
<tr class="header">
<th><blockquote>
<p><strong>Internal</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>User-facing</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p>CREATED / QUEUED</p>
</blockquote></th>
<th><blockquote>
<p>Queued</p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p>CLAIMED / PREPARING</p>
</blockquote></th>
<th><blockquote>
<p>Preparing</p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p>RUNNING</p>
</blockquote></th>
<th><blockquote>
<p>Generating</p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p>VALIDATING</p>
</blockquote></th>
<th><blockquote>
<p>Checking</p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p>COMPLETED</p>
</blockquote></th>
<th><blockquote>
<p>Completed</p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p>RETRY_PENDING</p>
</blockquote></th>
<th><blockquote>
<p>Retrying</p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p>FAILED</p>
</blockquote></th>
<th><blockquote>
<p>Failed</p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p>CANCELLED</p>
</blockquote></th>
<th><blockquote>
<p>Cancelled</p>
</blockquote></th>
</tr>
</thead>
<tbody>
</tbody>
</table>

# **11. Job Response**

{

"status": "success",

"data": {

"job": {

"jobId": "...",

"operationType": "IMAGE_GENERATION",

"status": "RUNNING",

"target": {

"type": "SCENE",

"id": "..."

},

"progress": {

"stage": "GENERATING",

"percent": 55,

"message": "Creating scene artwork."

},

"attempt": {

"current": 1,

"maximum": 3

},

"createdAt": "...",

"startedAt": "...",

"completedAt": null

}

}

}

# **12. Progress Model**

Progress gồm:

stage

percent

message

percent là optional.

Không phải AI model nào cũng cho progress chính xác.

# **13. Recommended Progress Stages**

Image generation:

QUEUED

PREPARING_CONTEXT

LOADING_REFERENCES

GENERATING

VALIDATING

SAVING_RESULT

COMPLETED

# **14. Progress Accuracy Rule**

Nếu backend không biết chính xác 63% hay 64% thì không được tạo cảm giác chính xác giả.

Có thể chỉ trả:

{

"stage": "GENERATING",

"percent": null,

"message": "Creating artwork."

}

UI có thể dùng indeterminate animation.

# **15. Polling**

Frontend luôn có thể poll:

GET /api/v1/jobs/{jobId}

MVP có thể poll mỗi:

2–5 seconds

tùy operation.

Không poll vài chục lần mỗi giây.

# **16. Polling Completion**

Khi Job:

COMPLETED

FAILED

REJECTED

CANCELLED

frontend dừng polling.

# **17. Job Result**

Khi completed:

{

"status": "success",

"data": {

"job": {

"jobId": "...",

"status": "COMPLETED",

"completedAt": "..."

},

"result": {

"candidateIds": \[

"candidate-1",

"candidate-2",

"candidate-3",

"candidate-4"

\]

}

}

}

Candidate details có thể được lấy riêng.

# **18. Why Not Put Full Images in Job Response**

Job resource không nên trả tất cả candidate metadata và signed URLs nếu không cần.

Job chỉ nói:

> công việc đã xong và tạo những Candidate nào.

Frontend có thể gọi:

GET /api/v1/scenes/{sceneId}/candidates

# **19. SignalR**

OWNIVERSE có thể sử dụng **SignalR** để cập nhật realtime.

Ví dụ connection:

/api/realtime

hoặc hub route phù hợp implementation.

# **20. Realtime Event Envelope**

Realtime event sử dụng format riêng:

{

"eventType": "JOB_PROGRESS",

"eventId": "event-id",

"occurredAt": "2026-09-13T15:52:00Z",

"data": {}

}

# **21. Why Events Do Not Use JSend**

JSend mô tả:

> kết quả của một HTTP request.

Realtime event mô tả:

> một sự kiện đã xảy ra.

Do đó event cần eventType, không cần status: success.

# **22. Event Types**

Core event types:

JOB_QUEUED

JOB_STARTED

JOB_PROGRESS

JOB_RETRYING

JOB_COMPLETED

JOB_FAILED

JOB_REJECTED

JOB_CANCELLED

CANDIDATE_AVAILABLE

OUTPUT_STALE

# **23. JOB_PROGRESS Event**

{

"eventType": "JOB_PROGRESS",

"eventId": "...",

"occurredAt": "...",

"data": {

"jobId": "...",

"status": "RUNNING",

"progress": {

"stage": "GENERATING",

"percent": 55,

"message": "Creating scene artwork."

}

}

}

# **24. JOB_COMPLETED Event**

{

"eventType": "JOB_COMPLETED",

"eventId": "...",

"occurredAt": "...",

"data": {

"jobId": "...",

"target": {

"type": "SCENE",

"id": "..."

},

"candidateIds": \[

"...",

"..."

\]

}

}

# **25. JOB_FAILED Event**

{

"eventType": "JOB_FAILED",

"eventId": "...",

"occurredAt": "...",

"data": {

"jobId": "...",

"error": {

"errorKey": "AI_GENERATION_FAILED",

"message": "Artwork generation could not be completed."

}

}

}

Không gửi raw stack trace.

# **26. Candidate Available Event**

Nếu Candidate xuất hiện từng cái một:

{

"eventType": "CANDIDATE_AVAILABLE",

"eventId": "...",

"occurredAt": "...",

"data": {

"jobId": "...",

"candidateId": "...",

"target": {

"type": "SCENE",

"id": "..."

}

}

}

MVP không bắt buộc stream Candidate từng cái.

# **27. Event Subscription Scope**

Frontend chỉ nên nhận event liên quan user/project mà user có quyền.

Không broadcast mọi Job cho mọi connected client.

# **28. Reconnect Rule**

Nếu SignalR disconnect:

Frontend reconnect.

Sau reconnect:

> không giả định đã nhận đủ event.

Frontend phải query lại:

GET /api/v1/jobs/{jobId}

để đồng bộ trạng thái.

# **29. Source of Truth Rule**

**API2-RULE-01**

> The Job API is authoritative; realtime events are advisory.

Nếu event nói RUNNING nhưng GET Job nói COMPLETED:

Frontend tin Job resource.

# **30. Cancel Job**

Endpoint:

POST /api/v1/jobs/{jobId}/cancel

# **31. Cancel Request**

Request có thể là:

{}

hoặc không cần payload.

Success:

{

"status": "success",

"data": {

"job": {

"jobId": "...",

"status": "CANCEL_REQUESTED"

}

}

}

# **32. Cancellation Is Not Instant**

Nếu model đang inference:

cancel có thể cần đợi safe checkpoint.

Do đó:

RUNNING

→ CANCEL_REQUESTED

→ CANCELLED

thay vì nhảy trực tiếp.

# **33. Cannot Cancel Completed Job**

Nếu Job đã completed:

422 Unprocessable Entity

{

"status": "fail",

"data": {

"errorKey": "JOB_NOT_CANCELLABLE",

"message": "This job can no longer be cancelled."

}

}

# **34. Cancelled Output**

Partial output của cancelled Job:

- không selectable;

- không canonical;

- không xuất hiện như Candidate hợp lệ.

Temporary artifacts được cleanup.

# **35. Retry**

Retry xảy ra do runtime failure.

Ví dụ:

GPU temporarily unavailable

Network timeout

Storage timeout

Không phải user action mặc định.

# **36. Retry State**

RUNNING

↓

Transient Failure

↓

RETRY_PENDING

↓

QUEUED

↓

New Attempt

Job ID giữ nguyên.

Attempt ID mới.

# **37. Retry Progress Event**

{

"eventType": "JOB_RETRYING",

"eventId": "...",

"occurredAt": "...",

"data": {

"jobId": "...",

"nextAttempt": 2,

"maxAttempts": 3,

"message": "Retrying generation after a temporary failure."

}

}

# **38. Retry vs Regeneration**

Đây là distinction bắt buộc.

**Retry**

same Job

same user intent

new Attempt

**Regeneration**

new Job

new creative attempt

new Candidate

# **39. Example Retry**

User yêu cầu:

> Generate Scene 10.

Attempt 1 bị network timeout.

System retry:

Job J100

├── Attempt 1 FAILED

└── Attempt 2 RUNNING

# **40. Example Regeneration**

User không thích Candidate.

User bấm Regenerate.

Job J100 → old generation

Job J101 → regeneration

Đây là hai Job khác nhau.

# **41. Retry Limit**

Job có:

currentAttempt

maxAttempts

Ví dụ:

{

"currentAttempt": 2,

"maxAttempts": 3

}

Không retry vô hạn.

# **42. Retryable Errors**

Ví dụ:

AI_PROVIDER_TIMEOUT

TEMPORARY_GPU_ERROR

STORAGE_TEMPORARILY_UNAVAILABLE

NETWORK_ERROR

WORKER_LOST

# **43. Non-Retryable Errors**

Ví dụ:

INVALID_CONTEXT

UNSUPPORTED_OPERATION

MISSING_CHARACTER_REFERENCE

POLICY_REJECTED

INVALID_MODEL_CONFIGURATION

# **44. Failed Job**

Khi hết retry:

FAILED

GET Job có thể trả:

{

"status": "success",

"data": {

"job": {

"jobId": "...",

"status": "FAILED",

"error": {

"errorKey": "AI_GENERATION_FAILED",

"message": "The generation could not be completed.",

"retryableByUser": true

}

}

}

}

Lưu ý đây vẫn là **JSend success** vì GET Job thành công.

Job bản thân nó có trạng thái FAILED.

# **45. Important Distinction**

Request:

GET /jobs/J100

thành công.

Do đó:

"status": "success"

dù:

"job.status": "FAILED"

Không nhầm HTTP request failure với business Job failure.

# **46. Rejected Job**

REJECTED nghĩa là processing chạy được nhưng output không thể được chấp nhận.

Ví dụ:

- mandatory consistency failure;

- safety validation failure;

- invalid output contract.

# **47. REJECTED Example**

{

"status": "success",

"data": {

"job": {

"jobId": "...",

"status": "REJECTED",

"error": {

"errorKey": "OUTPUT_VALIDATION_REJECTED",

"message": "The generated output did not pass validation."

}

}

}

}

# **48. User-Initiated Retry**

Nếu Job failed và product cho phép user “Try Again”:

khuyến nghị tạo:

> **new Job**

thay vì revive Job cũ.

Lý do:

- Job cũ giữ nguyên history;

- request mới rõ ràng;

- provenance dễ hiểu.

# **49. Idempotency**

Generation POST hỗ trợ:

Idempotency-Key

Ví dụ:

Idempotency-Key: d6cf5b8d...

# **50. Duplicate Generation Request**

Nếu cùng Idempotency-Key được gửi lại với cùng payload:

backend trả Job đã tạo trước đó.

{

"status": "success",

"data": {

"job": {

"jobId": "existing-job",

"status": "QUEUED"

}

}

}

Không tạo Job mới.

# **51. Idempotency Conflict**

Nếu cùng key nhưng payload khác:

409 Conflict

{

"status": "fail",

"data": {

"errorKey": "IDEMPOTENCY_KEY_CONFLICT",

"message": "The same idempotency key was used with a different request."

}

}

# **52. Context Snapshot Contract**

Frontend không gửi Context Snapshot hoàn chỉnh.

Frontend chỉ gửi creative intent.

Backend chịu trách nhiệm:

Validate request

↓

Resolve Story state

↓

Create Context Snapshot

↓

Create Job

# **53. Why Frontend Does Not Build Snapshot**

Nếu frontend tự xây context:

- có thể thiếu Fact;

- có thể dùng Character version sai;

- có thể giả dữ liệu;

- khó bảo vệ Canon;

- research reproducibility kém.

Context phải do backend/AI application layer xây.

# **54. Context Outdated During Job**

Nếu user sửa Character sau khi Job đã bắt đầu:

running Job vẫn dùng original Snapshot.

Job không restart tự động.

# **55. Context Outdated After Completion**

Candidate có thể được đánh dấu:

POTENTIALLY_STALE

nếu canonical data đã đổi.

# **56. OUTPUT_STALE Event**

Ví dụ:

{

"eventType": "OUTPUT_STALE",

"eventId": "...",

"occurredAt": "...",

"data": {

"candidateId": "...",

"target": {

"type": "SCENE",

"id": "..."

},

"staleStatus": "REVIEW_RECOMMENDED",

"message": "Generated with older character data."

}

}

# **57. Stale Event Is Advisory**

Receiving stale event không tự:

- deselect Candidate;

- delete Candidate;

- regenerate output.

User quyết định.

# **58. Internal Worker Contract**

Frontend không truy Worker trực tiếp.

Logical flow:

Backend

↓

Queue

↓

Worker

Worker nhận identifier, không nhận arbitrary client payload trực tiếp.

# **59. Queue Message Contract**

Minimal message:

{

"jobId": "...",

"attemptId": "...",

"operationType": "IMAGE_GENERATION",

"contractVersion": 1

}

# **60. Why Queue Message Is Small**

Worker dùng jobId để resolve authoritative job/snapshot data.

Không nhét toàn Story vào message.

# **61. Worker Claim**

Internal worker may perform:

Claim Job

Start Attempt

Update Progress

Complete Attempt

Fail Attempt

Heartbeat

Các operation này không nằm trong public API.

# **62. Internal Worker API**

Nếu implementation dùng HTTP giữa worker và backend, có thể có private endpoints như:

/internal/v1/jobs/{jobId}/claim

/internal/v1/jobs/{jobId}/progress

/internal/v1/jobs/{jobId}/complete

/internal/v1/jobs/{jobId}/fail

Những endpoint này:

- không public;

- không accessible bởi frontend;

- yêu cầu service authentication.

# **63. Worker Authentication**

Worker dùng:

- service token;

- private network identity;

- signed service credential;

không dùng user token.

# **64. Worker Progress Payload**

Logical example:

{

"attemptId": "...",

"stage": "GENERATING",

"percent": 50,

"message": "Creating artwork."

}

# **65. Worker Completion Payload**

Worker không nên gửi canonical Story update.

Completion chỉ chứa generated result references.

Ví dụ:

{

"attemptId": "...",

"results": \[

{

"candidateIndex": 0,

"assetId": "..."

}

\],

"validation": {}

}

# **66. Worker Canonical Boundary**

**API2-RULE-02**

> Worker SHALL NOT send arbitrary canonical entity mutation as part of generation completion.

AI runtime không được nói:

Update Character eyeColor = green

chỉ vì ảnh sinh ra có mắt xanh lá.

# **67. Worker Failure Contract**

Logical payload:

{

"attemptId": "...",

"failure": {

"category": "TEMPORARY_GPU_ERROR",

"message": "GPU memory allocation failed.",

"retryable": true

}

}

Raw provider details có thể được log internal nhưng không expose public.

# **68. Job Error Categories**

Có thể chia:

VALIDATION

BUSINESS

AI_RUNTIME

PROVIDER

STORAGE

WORKER

TIMEOUT

CANCELLED

UNKNOWN

# **69. Public Error Normalization**

Internal error:

CUDA_OUT_OF_MEMORY

không nhất thiết trả frontend nguyên văn.

Frontend có thể nhận:

AI_RUNTIME_UNAVAILABLE

với message phù hợp.

# **70. Async HTTP Failure**

Ví dụ request generate không thể tạo Job vì Scene thiếu dữ liệu bắt buộc.

422 Unprocessable Entity

{

"status": "fail",

"data": {

"errorKey": "GENERATION_CONTEXT_INCOMPLETE",

"message": "The scene does not have enough information to start generation.",

"missing": \[

"scene.description"

\]

}

}

Job không được tạo.

# **71. Async Infrastructure Failure Before Job Creation**

Nếu queue/database unavailable trước khi Job được tạo:

503 Service Unavailable

{

"status": "error",

"message": "Generation service is temporarily unavailable.",

"code": 50310,

"data": {

"errorKey": "GENERATION_SERVICE_UNAVAILABLE",

"traceId": "..."

}

}

# **72. Failure After Job Creation**

Nếu Job đã được tạo rồi Worker gặp lỗi:

HTTP request ban đầu vẫn đã thành công.

Failure được phản ánh trong:

Job.status = FAILED

không bằng cách thay đổi response HTTP cũ.

# **73. List Active Jobs**

Frontend có thể cần global generation indicator.

Endpoint:

GET /api/v1/jobs?status=active

hoặc:

GET /api/v1/projects/{projectId}/jobs?state=active

# **74. Active State Definition**

Active có thể map:

CREATED

QUEUED

CLAIMED

PREPARING

RUNNING

VALIDATING

RETRY_PENDING

CANCEL_REQUESTED

# **75. Scene Active Job**

Scene response có thể chứa summary:

{

"activeGeneration": {

"jobId": "...",

"status": "RUNNING",

"stage": "GENERATING"

}

}

để UX không cần gọi quá nhiều request.

# **76. Multiple Concurrent Jobs**

Một Scene có thể có nhiều generation Job trong history.

MVP có thể giới hạn:

1 active image generation per Scene

để tránh confusion.

# **77. Concurrent Job Failure**

Nếu đã có active Job và user gửi thêm khi policy không cho phép:

409 Conflict

{

"status": "fail",

"data": {

"errorKey": "GENERATION_ALREADY_RUNNING",

"message": "A generation is already running for this scene.",

"activeJobId": "..."

}

}

# **78. User-Level Concurrency Limit**

Có thể giới hạn:

N active image jobs / user

Nếu vượt:

429 Too Many Requests

# **79. Estimated Wait Time**

Không bắt buộc.

Nếu backend có dữ liệu đáng tin cậy có thể trả:

{

"queue": {

"position": 3,

"estimatedWaitSeconds": 45

}

}

Không trả estimate giả nếu không tính được.

# **80. Job History**

Endpoint:

GET /api/v1/projects/{projectId}/jobs

Pagination bắt buộc khi history lớn.

Có thể filter:

operationType

status

targetType

targetId

# **81. Job History Response**

{

"status": "success",

"data": {

"items": \[

{

"jobId": "...",

"operationType": "IMAGE_GENERATION",

"status": "COMPLETED",

"target": {

"type": "SCENE",

"id": "..."

},

"createdAt": "...",

"completedAt": "..."

}

\],

"pagination": {

"page": 1,

"pageSize": 20,

"totalItems": 57,

"totalPages": 3

}

}

}

# **82. Attempt Visibility**

Normal creator UI không cần xem tất cả Attempt.

Advanced/history endpoint có thể expose summary.

Ví dụ:

GET /api/v1/jobs/{jobId}/attempts

# **83. Attempt Response**

{

"status": "success",

"data": {

"items": \[

{

"attemptNumber": 1,

"status": "FAILED",

"failureCategory": "TEMPORARY_PROVIDER_ERROR"

},

{

"attemptNumber": 2,

"status": "COMPLETED"

}

\]

}

}

Không expose sensitive model credentials/config secrets.

# **84. Job Cancellation Authorization**

User chỉ cancel Job thuộc Project họ có quyền.

Worker/system maintenance có thể cancel theo internal privilege.

# **85. Idempotent Cancel**

Nếu Job đã:

CANCEL_REQUESTED

và client gửi cancel lại:

backend có thể trả success với current state.

Không cần tạo error.

# **86. Job State Transition Validation**

Backend phải từ chối internal transition không hợp lệ.

Ví dụ:

COMPLETED → RUNNING

không được phép.

# **87. Public API Cannot Set Job Status**

Không có:

PATCH /jobs/{id}

{

"status": "COMPLETED"

}

cho frontend.

Job status chỉ được thay đổi bởi authorized runtime workflow.

# **88. Realtime Ordering**

Event có thể đến chậm hoặc out-of-order do network.

Frontend không nên suy luận canonical Job state chỉ từ thứ tự event.

Có thể sử dụng:

occurredAt

và query Job resource khi cần.

# **89. Event ID**

Mỗi event có:

eventId

giúp client/debug xác định duplicate event.

Frontend có thể ignore duplicate event nếu eventId đã xử lý.

# **90. Event Contract Versioning**

Có thể thêm:

{

"eventVersion": 1

}

nếu event schema dự kiến thay đổi mạnh.

MVP có thể mặc định version 1.

# **91. Realtime Error**

SignalR connection error không đồng nghĩa Job failed.

Nếu realtime mất:

UI → polling fallback

Job vẫn chạy.

# **92. Candidate Availability After Reconnect**

User refresh browser.

Frontend:

GET Scene

↓

GET Active Job if any

↓

GET Candidates

Không phụ thuộc event đã được nhận trước refresh.

# **93. Timeout**

Job timeout được xử lý runtime.

Nếu attempt timeout:

Attempt FAILED

và có thể:

Job → RETRY_PENDING

nếu policy cho phép.

# **94. Timeout Public Message**

{

"errorKey": "GENERATION_TIMEOUT",

"message": "The generation took too long and could not be completed."

}

Không cần nói chi tiết timeout internal là bao nhiêu nếu không hữu ích.

# **95. Batch Jobs**

Future:

BatchJob

├── Child Job 1

├── Child Job 2

└── Child Job N

MVP không bắt buộc.

Nếu hỗ trợ, parent Job có aggregate progress.

# **96. Batch Progress**

Ví dụ:

{

"completed": 7,

"total": 10

}

khác với model inference percent.

# **97. Error Contract — HTTP**

HTTP error tiếp tục dùng API-01 convention.

Ví dụ:

{

"status": "fail",

"data": {

"errorKey": "...",

"message": "..."

}

}

hoặc:

{

"status": "error",

"message": "...",

"code": 50001,

"data": {

"errorKey": "...",

"traceId": "..."

}

}

# **98. Error Contract — Job**

Job error nằm trong Job resource:

{

"jobId": "...",

"status": "FAILED",

"error": {

"errorKey": "AI_GENERATION_FAILED",

"message": "...",

"retryableByUser": true

}

}

Hai loại error này phải phân biệt.

# **99. HTTP Error vs Job Error**

Ví dụ:

### **HTTP error**

Frontend không tạo được Job.

POST generation → 422

### **Job error**

Job đã tạo thành công nhưng AI fail sau 30 giây.

POST generation → 202

later

GET Job → job.status = FAILED

# **100. Security Requirements**

**API2-SEC-01  
**Public clients SHALL NOT directly access worker endpoints.

**API2-SEC-02  
**Realtime subscriptions SHALL be authorized.

**API2-SEC-03  
**User SHALL only receive Job events for authorized Projects.

**API2-SEC-04  
**Internal worker credentials SHALL not be exposed to frontend.

**API2-SEC-05  
**Raw provider/model errors SHALL not be exposed by default.

# **101. Functional Requirements**

**API2-FR-01  
**Long-running AI operations SHALL return HTTP 202 with a Job.

**API2-FR-02  
**Job SHALL be queryable independently of realtime events.

**API2-FR-03  
**HTTP responses SHALL use JSend.

**API2-FR-04  
**Realtime events SHALL use event envelope rather than JSend.

**API2-FR-05  
**Job progress SHALL expose logical stage.

**API2-FR-06  
**System SHALL support Job cancellation where operation allows.

**API2-FR-07  
**Runtime retry SHALL create a new Attempt, not a new Job.

**API2-FR-08  
**User regeneration SHALL create a new Job.

**API2-FR-09  
**Frontend SHALL recover Job state after reconnect.

**API2-FR-10  
**Duplicate generation request SHALL support idempotency control.

**API2-FR-11  
**Worker SHALL not directly mutate canonical Story state.

**API2-FR-12  
**Completed Job SHALL expose generated Candidate references.

**API2-FR-13  
**Job failures SHALL retain diagnostic metadata.

**API2-FR-14  
**Stale Candidate notifications SHALL not automatically mutate selection.

# **102. Acceptance Criteria**

### **AC-API2-01**

Given user starts image generation,  
when request is accepted,  
then HTTP response SHALL return 202, JSend success, and Job ID.

### **AC-API2-02**

Given frontend refreshes while generation is running,  
when it queries Job ID,  
then current Job status SHALL still be available.

### **AC-API2-03**

Given realtime connection disconnects,  
then Job SHALL continue processing.

### **AC-API2-04**

Given realtime reconnects,  
then frontend SHALL be able to synchronize current state using HTTP API.

### **AC-API2-05**

Given transient worker failure,  
when retry policy allows,  
then same Job SHALL create a new Attempt.

### **AC-API2-06**

Given user clicks Regenerate,  
then system SHALL create a new Job rather than reuse old Job.

### **AC-API2-07**

Given Job is already completed,  
when user requests cancellation,  
then API SHALL return JSend fail.

### **AC-API2-08**

Given duplicate request with same Idempotency-Key and same payload,  
then system SHALL return original Job rather than create duplicate work.

### **AC-API2-09**

Given Job completes,  
then Candidate IDs SHALL be retrievable even if realtime completion event was missed.

### **AC-API2-10**

Given Character changes after Candidate generation,  
then Candidate may become stale but SHALL not be automatically deleted or regenerated.

# **103. Main End-to-End Flow**

User clicks Generate

↓

POST /scenes/{id}/generations

↓

202 + Job ID

↓

Job QUEUED

↓

Worker claims Job

↓

PREPARING

↓

RUNNING

↓

SignalR progress events

↓

VALIDATING

↓

Candidate saved

↓

Job COMPLETED

↓

JOB_COMPLETED event

↓

Frontend GET candidates

↓

User selects Candidate

Giải thích bằng lời:

HTTP chỉ khởi động tác vụ. Job là đối tượng theo dõi xuyên suốt. Worker thực hiện AI generation. Frontend có thể nhận realtime progress, nhưng nếu mất kết nối thì vẫn lấy trạng thái qua REST API. Sau khi hoàn tất, user mới review và chọn Candidate.

# **104. Failure Flow**

Job RUNNING

↓

Failure

↓

Retryable?

┌───────┴───────┐

Yes No

↓ ↓

RETRY_PENDING FAILED

↓

New Attempt

↓

RUNNING

Nếu vượt maxAttempts:

FAILED

# **105. Contract Summary**

Có thể nhớ API-02 bằng bốn contract chính:

**1. Command Contract**

Start / Cancel

**2. Job Contract**

What is happening now?

**3. Event Contract**

Something just changed.

**4. Result Contract**

Which Candidates were produced?

# **106. Relationship With API-01**

**API-01** trả lời:

> Frontend thao tác với sản phẩm như thế nào?

**API-02** trả lời:

> Những tác vụ chạy lâu được theo dõi và giao tiếp như thế nào?

Hai tài liệu dùng chung:

- REST;

- /api/v1;

- camelCase;

- authorization;

- revision principles;

- JSend HTTP envelope;

- standardized errorKey.

# **107. Core Principle**

Nguyên tắc cuối cùng:

> **A generation request is not the generation itself; it creates a durable Job that the system can track, recover, retry and explain.**

Nói đơn giản:

**User có thể đóng màn hình, refresh trang, worker có thể retry, realtime có thể mất kết nối — nhưng Job vẫn phải tồn tại và hệ thống vẫn biết generation đang ở đâu.**
