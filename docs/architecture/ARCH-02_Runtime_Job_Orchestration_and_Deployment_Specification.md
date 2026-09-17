# **ARCH-02 — Runtime, Job Orchestration & Deployment Specification**

**Document ID: ARCH-02  
Document Type: Runtime, Job Orchestration & Deployment Specification  
Product: OWNIVERSE  
Project: Character-Consistent Long-Range Story Generation  
Primary Roles: Solution Architect / Cloud Architect / Backend Architect  
Status: Draft v1  
Parent Document: ARCH-01 — Solution Architecture Specification  
Related Documents: DATA-01, API-01, API-02, AI-01, AI-02, AI-03**

# **1. Purpose**

**Tài liệu này đặc tả cách OWNIVERSE vận hành tại runtime và cách các tác vụ dài, đặc biệt là AI generation, được điều phối ngoài request-response thông thường.**

**Tài liệu trả lời các câu hỏi:**

- **generation request được biến thành background job như thế nào;**

- **job đi qua những trạng thái nào;**

- **queue và worker phối hợp ra sao;**

- **retry, timeout và cancellation được xử lý thế nào;**

- **nhiều generation chạy đồng thời được kiểm soát như thế nào;**

- **frontend nhận progress bằng cách nào;**

- **AI worker và GPU workload được triển khai ra sao;**

- **hệ thống phục hồi như thế nào khi một service bị crash;**

- **deployment cho local development, thesis demo và production-like environment khác nhau thế nào.**

# **2. Scope**

**ARCH-02 bao gồm:**

- **runtime topology;**

- **background job execution;**

- **queue architecture;**

- **job lifecycle;**

- **job orchestration;**

- **retry;**

- **regeneration boundary;**

- **timeout;**

- **cancellation;**

- **concurrency;**

- **worker leasing;**

- **progress reporting;**

- **realtime notification;**

- **resource management;**

- **GPU scheduling;**

- **deployment environments;**

- **health checks;**

- **graceful shutdown;**

- **fault recovery;**

- **scaling;**

- **operational monitoring.**

# **3. Out of Scope**

**Tài liệu này không định nghĩa:**

- **database schema chi tiết;**

- **API endpoint cụ thể;**

- **prompt content;**

- **model architecture;**

- **AI research algorithm;**

- **UI visual details;**

- **exact cloud vendor.**

# **4. Runtime Principles**

## **RT-PR-01 — Long-running work is asynchronous**

**Các AI operation có thể chạy lâu không được giữ HTTP request mở cho toàn bộ thời gian inference.**

## **RT-PR-02 — Acknowledgement first, processing later**

**Backend phải có khả năng xác nhận request nhanh:**

**Request accepted**

**→ Job created**

**→ Job ID returned**

**Sau đó generation được xử lý phía sau.**

## **RT-PR-03 — Job state is persistent**

**Job state phải được lưu ở persistence layer.**

**Queue không phải nguồn sự thật duy nhất của job.**

## **RT-PR-04 — Workers are replaceable**

**Worker có thể crash hoặc restart mà hệ thống vẫn có khả năng xác định job đang ở trạng thái nào.**

## **RT-PR-05 — Retry is bounded**

**Không được retry vô hạn.**

## **RT-PR-06 — Runtime failure must not mutate canonical story state**

**Canonical Story, Character và Scene chỉ được update qua application workflow hợp lệ.**

# **5. Runtime Topology**

**Runtime tối thiểu gồm:**

**Browser**

**↓**

**Frontend**

**↓**

**ASP.NET Application API**

**↓**

**Application Database**

**↓**

**Job Queue**

**↓**

**Python AI Worker**

**↓**

**AI Model / GPU Runtime**

**↓**

**Validation**

**↓**

**Asset Storage**

**Ngoài ra có thể có:**

**Redis / Cache**

**Realtime Hub**

**Monitoring**

**Logging**

# **6. Request Types**

**Runtime chia request thành hai nhóm.**

## **6.1 Synchronous Operations**

**Ví dụ:**

- **create project;**

- **edit story;**

- **create character;**

- **update scene;**

- **select candidate;**

- **archive project.**

**Đặc điểm:**

- **xử lý nhanh;**

- **trả response trực tiếp;**

- **không cần queue.**

## **6.2 Asynchronous Operations**

**Ví dụ:**

- **image generation;**

- **character reference generation;**

- **large text generation;**

- **consistency validation;**

- **batch regeneration;**

- **expensive preprocessing.**

**Đặc điểm:**

- **tạo job;**

- **chạy qua worker;**

- **frontend theo dõi progress.**

# **7. Job Creation Flow**

**Frontend**

**↓**

**Generate Request**

**↓**

**Application API**

**↓**

**Validate Request**

**↓**

**Create Context Snapshot**

**↓**

**Create Job Record**

**↓**

**Publish Queue Message**

**↓**

**Return Job ID**

**Giải thích bằng lời:**

**Backend trước tiên kiểm tra request và quyền của user. Sau đó context liên quan được freeze thành snapshot. Job được lưu database trước khi message được đưa vào queue. Frontend nhận Job ID để theo dõi tiến trình.**

# **8. Why Persist Job Before Queue Publish**

**Job record phải tồn tại trước hoặc đồng thời với việc publish queue message.**

**Mục tiêu:**

- **không có queue message không truy được job;**

- **có thể audit;**

- **recovery dễ hơn;**

- **frontend có thể query status ngay sau khi request được chấp nhận.**

# **9. Job Core Model**

**Một Job tối thiểu có:**

**jobId**

**requestId**

**projectId**

**storyId**

**targetType**

**targetId**

**operationType**

**status**

**priority**

**contextSnapshotId**

**createdAt**

**queuedAt**

**startedAt**

**completedAt**

**currentAttempt**

**maxAttempts**

**progress**

**errorCode**

**errorMessage**

# **10. Job Lifecycle**

**Lifecycle chuẩn:**

**CREATED**

**↓**

**QUEUED**

**↓**

**CLAIMED**

**↓**

**PREPARING**

**↓**

**RUNNING**

**↓**

**VALIDATING**

**↓**

**COMPLETED**

**Các nhánh khác:**

**RETRY_PENDING**

**FAILED**

**CANCEL_REQUESTED**

**CANCELLED**

**REJECTED**

# **11. Status Definitions**

## **CREATED**

**Job record đã tồn tại nhưng chưa publish thành công vào queue.**

## **QUEUED**

**Job đang chờ worker.**

## **CLAIMED**

**Một worker đã nhận quyền xử lý job.**

## **PREPARING**

**Worker đang load context, reference assets hoặc model dependencies.**

## **RUNNING**

**AI inference đang thực hiện.**

## **VALIDATING**

**Output đang được kiểm tra.**

## **COMPLETED**

**Job hoàn thành và output hợp lệ đã được persistence.**

## **RETRY_PENDING**

**Job sẽ được chạy lại do transient failure.**

## **FAILED**

**Không thể tiếp tục sau retry policy.**

## **REJECTED**

**Generation tạo được output nhưng output không đạt mandatory validation.**

## **CANCEL_REQUESTED**

**User/system đã yêu cầu cancel nhưng worker chưa xác nhận dừng.**

## **CANCELLED**

**Job đã dừng.**

# **12. State Transition Rules**

**Không được cho phép state transition tùy ý.**

**Ví dụ hợp lệ:**

**QUEUED → CLAIMED**

**CLAIMED → RUNNING**

**RUNNING → VALIDATING**

**VALIDATING → COMPLETED**

**Ví dụ không hợp lệ:**

**CREATED → COMPLETED**

**FAILED → RUNNING**

**Nếu cần chạy lại failed job, phải tạo retry attempt hoặc job mới theo policy.**

# **13. Generation Attempt**

**Job và Attempt phải phân biệt.**

**Job**

**├── Attempt 1**

**├── Attempt 2**

**└── Attempt 3**

**Một Job biểu diễn user intent.**

**Attempt biểu diễn một lần execution cụ thể.**

# **14. Attempt Model**

**Attempt tối thiểu gồm:**

**attemptId**

**jobId**

**attemptNumber**

**workerId**

**startedAt**

**completedAt**

**modelId**

**modelVersion**

**parameters**

**status**

**failureCategory**

# **15. Retry vs Regeneration**

**Hai khái niệm không được trộn.**

## **Retry**

**Cùng một generation intent.**

**Nguyên nhân thường:**

- **network timeout;**

- **provider unavailable;**

- **temporary GPU error.**

## **Regeneration**

**Tạo output mới có chủ đích.**

**Có thể thay đổi:**

- **seed;**

- **prompt;**

- **context;**

- **instruction;**

- **model parameters.**

**Retry thuộc runtime recovery.**

**Regeneration thuộc product workflow.**

# **16. Queue Architecture**

**Queue chịu trách nhiệm:**

- **giữ pending jobs;**

- **phân phối cho workers;**

- **kiểm soát concurrency;**

- **tách API khỏi AI runtime;**

- **hỗ trợ retry.**

**Queue message chỉ nên chứa identifier cần thiết.**

**Ví dụ:**

**jobId**

**attemptId**

**operationType**

**Không cần serialize toàn bộ Story vào message.**

# **17. Queue Message Minimalism**

**Không nên đưa toàn bộ context vào queue message vì:**

- **message có thể rất lớn;**

- **dữ liệu khó version;**

- **retry khó;**

- **security phức tạp;**

- **duplication cao.**

**Worker dùng Job ID để load immutable snapshot.**

# **18. Queue Types**

**MVP có thể bắt đầu bằng một queue chung:**

**ai-jobs**

**Khi cần, có thể tách:**

**text-generation**

**image-generation**

**validation**

**batch**

**Không bắt buộc chia queue quá sớm.**

# **19. Job Priority**

**Priority có thể gồm:**

**LOW**

**NORMAL**

**HIGH**

**MVP có thể mặc định NORMAL.**

**Priority nên dùng cho:**

- **interactive user generation;**

- **background batch task;**

- **maintenance task.**

# **20. Worker Claim**

**Worker nhận job theo cơ chế claim/lease.**

**Một job đang được worker xử lý phải có:**

**workerId**

**leaseUntil**

**heartbeatAt**

# **21. Worker Lease**

**Lease giúp hệ thống phát hiện:**

> **Worker đã nhận job nhưng bị crash.**

**Nếu lease hết hạn và không còn heartbeat:**

**Job có thể được đưa về recovery flow.**

# **22. Heartbeat**

**Worker đang chạy job dài nên gửi heartbeat định kỳ.**

**Heartbeat cho biết:**

- **worker còn sống;**

- **job còn đang chạy;**

- **inference chưa bị bỏ rơi.**

# **23. Orphan Job Detection**

**Một job được xem là orphan nếu:**

- **state đang CLAIMED/RUNNING;**

- **lease đã hết;**

- **worker không heartbeat.**

**Recovery manager có thể:**

1.  **kiểm tra attempt;**

2.  **đánh dấu attempt thất bại;**

3.  **đưa job sang RETRY_PENDING;**

4.  **retry nếu policy cho phép.**

# **24. Worker Lifecycle**

**Worker runtime:**

**Start**

**↓**

**Load Configuration**

**↓**

**Initialize Model**

**↓**

**Register Worker**

**↓**

**Poll Queue**

**↓**

**Claim Job**

**↓**

**Execute**

**↓**

**Report Result**

**↓**

**Release Job**

**↓**

**Poll Next Job**

# **25. Model Warm-Up**

**Với model lớn, worker có thể load model khi startup thay vì mỗi job.**

**Mục tiêu:**

- **giảm latency;**

- **tránh reload model;**

- **giảm GPU fragmentation.**

**Worker readiness chỉ nên healthy sau khi model bắt buộc đã load thành công.**

# **26. Worker Types**

**Có thể tồn tại:**

## **Text Worker**

**Xử lý LLM/text tasks.**

## **Image Worker**

**Xử lý diffusion/image tasks.**

## **Validation Worker**

**Xử lý image analysis/consistency.**

**MVP có thể gộp nếu tài nguyên cho phép.**

# **27. GPU Worker**

**GPU worker cần biết:**

- **GPU device;**

- **available memory;**

- **loaded model;**

- **maximum concurrency.**

**Không nên chạy quá nhiều inference đồng thời trên GPU nếu gây out-of-memory.**

# **28. GPU Concurrency**

**MVP nên dùng conservative policy:**

**1 expensive image inference / GPU**

**hoặc mức concurrency đã benchmark thực tế.**

**Không đặt concurrency theo số CPU core.**

# **29. Resource Reservation**

**Trước khi nhận job, worker có thể kiểm tra:**

- **required model;**

- **required GPU memory;**

- **operation type.**

**Nếu không đủ resource, worker không claim job đó.**

# **30. Model-Specific Queue Routing**

**Nếu sau này có nhiều model:**

**image-storydiffusion**

**image-proposed-method**

**text-llm**

**Worker subscribe queue phù hợp.**

**MVP có thể dùng route metadata thay vì nhiều queue.**

# **31. Context Loading**

**Worker không tự truy cập live Story state để tạo input mới.**

**Worker load:**

**Context Snapshot**

**Input Snapshot**

**Reference Asset IDs**

**Điều này đảm bảo reproducibility.**

# **32. Snapshot Integrity**

**Snapshot phải immutable.**

**Nếu user chỉnh Story trong lúc job chạy:**

**running job không thay input.**

# **33. Progress Reporting**

**Progress có thể được chia theo logical stage:**

**10% Preparing**

**30% Loading references**

**40% Generating**

**80% Validating**

**100% Complete**

**Nếu inference model không cung cấp progress đáng tin cậy, UI nên dùng stage-based progress thay vì giả phần trăm chính xác.**

# **34. Runtime Progress Model**

**Job có thể lưu:**

**progressStage**

**progressPercent**

**progressMessage**

**updatedAt**

**progressPercent là optional.**

# **35. Frontend Progress Delivery**

**Có ba lựa chọn:**

1.  **Polling;**

2.  **Server-Sent Events;**

3.  **WebSocket / SignalR.**

# **36. Recommended MVP Progress Strategy**

**Với ASP.NET Core:**

**SignalR hoặc polling đều phù hợp.**

**Đề xuất:**

- **API vẫn là source of truth;**

- **SignalR dùng cho realtime convenience;**

- **frontend fallback sang polling nếu realtime connection mất.**

# **37. Realtime Event Examples**

**JobQueued**

**JobStarted**

**JobProgress**

**JobCompleted**

**JobFailed**

**CandidateAvailable**

# **38. Realtime Event Rule**

**Realtime notification không phải canonical source.**

**Nếu frontend bỏ lỡ event:**

**nó vẫn phải query API và lấy trạng thái hiện tại.**

# **39. Cancellation Flow**

**User có thể cancel nếu operation cho phép.**

**User presses Cancel**

**↓**

**API sets CANCEL_REQUESTED**

**↓**

**Worker detects cancellation**

**↓**

**Stop at safe point**

**↓**

**Cleanup temporary resources**

**↓**

**CANCELLED**

# **40. Cooperative Cancellation**

**Không phải model nào cũng có thể dừng ngay lập tức.**

**Do đó cancellation là cooperative.**

**Worker kiểm tra cancellation tại:**

- **trước model call;**

- **giữa pipeline stages;**

- **iteration checkpoints nếu model hỗ trợ.**

# **41. Cancellation Safety**

**Cancelled job:**

- **không update canonical state;**

- **temporary files phải được cleanup;**

- **output chưa hoàn chỉnh không được làm candidate hợp lệ.**

# **42. Timeout Policy**

**Mỗi operation type phải có timeout policy.**

**Ví dụ:**

**text generation**

**image generation**

**validation**

**có timeout khác nhau.**

**Không dùng một global timeout cho mọi AI operation.**

# **43. Timeout Behaviour**

**Khi timeout:**

1.  **attempt dừng nếu có thể;**

2.  **attempt đánh dấu timeout;**

3.  **retry policy được kiểm tra;**

4.  **job retry hoặc fail.**

# **44. Retry Classification**

**Retryable:**

- **temporary network failure;**

- **provider unavailable;**

- **transient storage error;**

- **worker crash;**

- **recoverable GPU runtime error.**

**Không retry mặc định:**

- **invalid input;**

- **missing mandatory data;**

- **policy violation;**

- **deterministic validation failure;**

- **unsupported model configuration.**

# **45. Retry Backoff**

**Retry phải dùng backoff.**

**Ví dụ:**

**Attempt 1**

**→ wait short interval**

**Attempt 2**

**→ wait longer**

**Attempt 3**

**→ final retry**

**Không hammer provider liên tục.**

# **46. Maximum Attempts**

**Mỗi job phải có:**

**maxAttempts**

**Default có thể là 3 cho transient infrastructure failure.**

**Exact value là configuration.**

# **47. Dead-Letter Handling**

**Job thất bại sau max attempts cần được:**

- **đánh dấu FAILED;**

- **giữ error metadata;**

- **giữ attempt history.**

**Nếu dùng message broker hỗ trợ dead-letter queue, failed messages có thể được đưa vào đó để operational inspection.**

# **48. Idempotency**

**Job processing phải giảm nguy cơ duplicate result.**

**Một attempt không được tạo duplicate canonical update khi message bị giao lại.**

# **49. At-Least-Once Delivery Assumption**

**Queue thường nên được xem như có thể deliver message nhiều hơn một lần.**

**Worker phải xử lý:**

**same job message arrives again**

**một cách an toàn.**

# **50. Job Lock**

**Trước khi xử lý:**

**worker phải xác nhận job chưa:**

- **completed;**

- **cancelled;**

- **claimed hợp lệ bởi worker khác.**

# **51. Candidate Persistence Idempotency**

**Candidate creation có thể dùng unique relation:**

**attemptId + candidateIndex**

**để tránh duplicate khi retry persistence.**

# **52. Asset Upload Recovery**

**Flow nên là:**

**Generate Image**

**↓**

**Write Temporary Artifact**

**↓**

**Upload Asset**

**↓**

**Create Asset Record**

**↓**

**Create Candidate Record**

**Nếu upload asset thất bại:**

**candidate không được đánh dấu complete.**

# **53. Temporary Files**

**Worker có thể dùng temporary disk.**

**Temporary file phải:**

- **scoped theo job;**

- **cleanup sau success;**

- **cleanup sau failure;**

- **không được xem là persistent storage.**

# **54. Validation Runtime**

**Validation có thể chạy:**

## **Inline**

**Ngay trong image worker sau generation.**

**Hoặc:**

## **Separate Job**

**Nếu validation quá nặng.**

**MVP có thể inline để đơn giản.**

# **55. Multi-Stage Job**

**Một image generation job logic có thể gồm:**

**Prepare**

**↓**

**Generate**

**↓**

**Persist Asset**

**↓**

**Validate**

**↓**

**Persist Candidate**

**↓**

**Complete**

**Các stage phải ghi progress.**

# **56. Fan-Out Generation**

**Nếu user yêu cầu 4 candidates, có hai chiến lược.**

## **Strategy A**

**Một job sinh 4 output tuần tự.**

## **Strategy B**

**Parent job tạo 4 child jobs.**

**MVP nên ưu tiên Strategy A hoặc limited parallelism để giảm orchestration complexity.**

# **57. Batch Regeneration**

**Batch regenerate nhiều scene nên dùng:**

**Batch Job**

**├── Scene Job 1**

**├── Scene Job 2**

**└── Scene Job N**

**Parent job theo dõi aggregate progress.**

**Không cần trong MVP đầu tiên nếu thời gian hạn chế.**

# **58. Concurrency Limits**

**Concurrency phải có giới hạn theo:**

- **system;**

- **worker;**

- **user;**

- **model.**

**Ví dụ:**

**max 1–2 image jobs / GPU**

**max N pending jobs / user**

**Exact values là configuration.**

# **59. Per-User Rate Limit**

**Rate limit giúp tránh một user gửi hàng trăm generation requests.**

**Policy có thể gồm:**

- **active jobs;**

- **queued jobs;**

- **requests per minute.**

# **60. Queue Backpressure**

**Nếu queue quá dài:**

**system phải có khả năng:**

- **từ chối hoặc defer request;**

- **thông báo expected delay;**

- **giới hạn thêm generation.**

**Không tiếp tục nhận vô hạn.**

# **61. Worker Health**

**Mỗi worker có health state:**

**STARTING**

**READY**

**BUSY**

**DEGRADED**

**UNHEALTHY**

# **62. Health Check Types**

## **Liveness**

**Process còn sống hay không.**

## **Readiness**

**Worker có sẵn sàng nhận job hay không.**

**Ví dụ model chưa load xong:**

**live = true**

**ready = false**

# **63. API Health**

**Application API cần health check cho:**

- **application;**

- **database;**

- **queue connectivity;**

- **optional storage connectivity.**

**Không nhất thiết gọi AI model mỗi lần health check.**

# **64. Graceful Shutdown**

**Khi worker shutdown:**

1.  **ngừng claim job mới;**

2.  **hoàn thành hoặc checkpoint job hiện tại nếu có thể;**

3.  **release lease;**

4.  **flush logs;**

5.  **shutdown model runtime.**

# **65. Deployment Environments**

**OWNIVERSE nên có ít nhất:**

**Development**

**Demo / Thesis**

**Production-like**

# **66. Development Environment**

**Mục tiêu:**

**developer chạy toàn bộ hệ thống dễ dàng.**

**Locked development topology:**

**Vue 3 + Vite + TypeScript Frontend**

**ASP.NET Core API**

**PostgreSQL**

**RabbitMQ**

**Python Worker**

**LocalFileAssetStorage thông qua IAssetStorage**

**Có thể chạy qua Docker Compose.**

**Development không cần dựng S3-compatible server chỉ để lưu file local. Storage provider phải có thể thay đổi bằng configuration mà không thay đổi Application/Domain code.**

# **67. Local AI Development**

**Nếu máy local không có GPU đủ mạnh:**

**AI worker có thể:**

- **gọi remote Colab/GPU runtime;**

- **dùng mock adapter;**

- **dùng lightweight development model.**

**Application backend không thay đổi.**

# **68. Mock AI Provider**

**Development cần hỗ trợ fake/mock provider.**

**Ví dụ:**

- **trả ảnh mẫu;**

- **delay 3 giây;**

- **mô phỏng failure;**

- **mô phỏng warning.**

**Điều này giúp frontend/backend phát triển mà không cần GPU chạy liên tục.**

# **69. Thesis Demo Environment**

**Mục tiêu:**

- **ổn định;**

- **reproducible;**

- **đủ mạnh để demo end-to-end.**

**Có thể dùng:**

**Web App Host**

**Database**

**Queue**

**Object Storage**

**Remote GPU Worker**

**GPU worker có thể chạy trên Colab Pro hoặc environment riêng nếu phù hợp với demo.**

# **70. Colab Constraint**

**Google Colab không nên được xem là production infrastructure lâu dài.**

**Nó phù hợp cho:**

- **research;**

- **inference experiments;**

- **thesis demonstration;**

- **temporary GPU worker.**

**Application architecture phải cho phép thay Colab worker bằng dedicated GPU environment sau này.**

# **71. Production-Like Topology**

**Reverse Proxy**

**↓**

**Frontend**

**↓**

**Application API**

**↓**

**Database**

**↓**

**Queue**

**↓**

**AI Worker Pool**

**↓**

**GPU Runtime**

**Asset Storage**

**Monitoring**

**Logging**

# **72. Containerization**

**Recommended deployable components:**

**frontend-container**

**api-container**

**worker-container**

**Database và queue có thể dùng managed service hoặc container tùy environment.**

# **73. Configuration Management**

**Configuration phải externalized.**

**Ví dụ:**

**DATABASE_URL**

**QUEUE_URL**

**ASSET_STORAGE_URL**

**AI_PROVIDER**

**MODEL_ID**

**MAX_RETRIES**

**WORKER_CONCURRENCY**

**Không hard-code environment-specific value trong source code.**

# **74. Secret Management**

**Secret gồm:**

- **API key;**

- **database credentials;**

- **storage credential;**

- **AI provider token.**

**Không commit secret vào source repository.**

# **75. Deployment Versioning**

**Mỗi deployable component nên có version.**

**Ví dụ:**

**apiVersion**

**workerVersion**

**modelVersion**

**Generation metadata nên ghi model/method version.**

# **76. Worker/API Compatibility**

**Worker và API phải giao tiếp bằng versioned contract.**

**Nếu worker version mới thay input schema:**

**compatibility phải được quản lý.**

**Không deploy schema-breaking worker tùy ý.**

# **77. Model Version Pinning**

**Research environment cần pin:**

- **model version;**

- **library version;**

- **checkpoint;**

- **configuration.**

**Điều này quan trọng cho reproducibility của luận văn.**

# **78. Deployment Rollback**

**Nếu API hoặc worker version mới lỗi:**

**system nên có khả năng rollback deployable component mà không mất canonical database state.**

# **79. Database Migration**

**Database migration phải độc lập với application startup nếu deployment nghiêm túc.**

**MVP có thể đơn giản hóa nhưng migration vẫn cần version-control.**

# **80. Observability**

**System cần thu thập:**

## **API Metrics**

- **request rate;**

- **latency;**

- **errors.**

## **Queue Metrics**

- **queue depth;**

- **oldest waiting job;**

- **retry count.**

## **Worker Metrics**

- **jobs processed;**

- **generation duration;**

- **GPU memory;**

- **worker errors.**

## **AI Metrics**

- **model latency;**

- **validation rejection;**

- **regeneration rate.**

# **81. Logging Context**

**Mọi log liên quan generation nên chứa:**

**jobId**

**attemptId**

**projectId**

**operationType**

**workerId**

**Không log toàn bộ private story content mặc định.**

# **82. Sensitive Logging**

**Không log:**

- **API key;**

- **user credentials;**

- **raw private data không cần thiết.**

**Prompt logging nếu cần research/debug phải có policy riêng.**

# **83. Alert Conditions**

**Production-like environment có thể alert khi:**

- **queue depth vượt threshold;**

- **worker không healthy;**

- **failure rate tăng;**

- **database unavailable;**

- **asset storage unavailable.**

# **84. Recovery After API Restart**

**Nếu API restart:**

- **queued jobs vẫn tồn tại;**

- **workers vẫn có thể xử lý nếu queue/persistence độc lập;**

- **frontend reconnect và query lại job state.**

# **85. Recovery After Worker Restart**

**Nếu worker restart:**

- **active lease eventually expires;**

- **orphan detection chạy;**

- **job được retry nếu policy cho phép.**

# **86. Recovery After Queue Failure**

**Vì database lưu Job state:**

**system có thể tìm các job:**

**CREATED**

**nhưng chưa publish thành công.**

**Recovery publisher có thể publish lại.**

# **87. Transactional Outbox Option**

**Nếu cần độ tin cậy cao hơn, có thể dùng Outbox Pattern:**

**Database Transaction**

**├── Create Job**

**└── Create Outbox Message**

**Background publisher sau đó đưa message vào queue.**

**MVP có thể triển khai sau nếu cần.**

# **88. Why Outbox Matters**

**Nó tránh tình trạng:**

**Job saved**

**but**

**queue publish failed**

**hoặc ngược lại.**

**Đây là enhancement tốt cho production-like architecture.**

# **89. Job Retention**

**Completed job metadata nên được giữ để hỗ trợ:**

- **history;**

- **debugging;**

- **research;**

- **audit.**

**Không xóa ngay khi job complete.**

# **90. Runtime Data Retention**

**Temporary runtime data có thể cleanup sớm.**

**Persistent data gồm:**

- **job metadata;**

- **attempt metadata;**

- **candidate;**

- **asset;**

- **provenance.**

# **91. Runtime Security**

**Worker không cần full application permission.**

**Nên dùng least privilege.**

**Ví dụ worker chỉ cần:**

- **read job snapshot;**

- **write attempt result;**

- **upload asset;**

- **update job status.**

# **92. Network Isolation**

**Nếu deployment hỗ trợ:**

- **database private;**

- **worker private;**

- **AI inference endpoint private;**

- **frontend chỉ truy public API.**

# **93. Acceptance Requirements**

## **ARCH2-FR-01**

**Long-running AI operation SHALL chạy qua background job.**

## **ARCH2-FR-02**

**Job SHALL có persistent state.**

## **ARCH2-FR-03**

**Worker SHALL sử dụng immutable snapshot.**

## **ARCH2-FR-04**

**System SHALL hỗ trợ bounded retry.**

## **ARCH2-FR-05**

**System SHALL phát hiện orphaned jobs.**

## **ARCH2-FR-06**

**Frontend SHALL có khả năng lấy trạng thái job sau khi reconnect.**

## **ARCH2-FR-07**

**Worker SHALL không update canonical Story state trực tiếp.**

## **ARCH2-FR-08**

**Generation output SHALL được persist trước khi job được đánh dấu completed.**

## **ARCH2-FR-09**

**Cancellation SHALL không làm corrupt canonical state.**

## **ARCH2-FR-10**

**AI worker SHALL có thể scale độc lập với Application API.**

# **94. Non-Functional Requirements**

## **ARCH2-NFR-01 — Reliability**

**Worker crash không được làm mất Job intent.**

## **ARCH2-NFR-02 — Recoverability**

**System phải có recovery path cho orphan job.**

## **ARCH2-NFR-03 — Scalability**

**Có thể tăng số worker mà không thay đổi frontend.**

## **ARCH2-NFR-04 — Observability**

**Mỗi generation phải trace được xuyên API → Queue → Worker.**

## **ARCH2-NFR-05 — Portability**

**Deployment không phụ thuộc cứng vào một cloud provider.**

## **ARCH2-NFR-06 — Reproducibility**

**Research generation phải lưu model/config version cần thiết.**

# **95. Acceptance Criteria**

### **AC-ARCH2-01**

**Given user request image generation,  
when API chấp nhận request,  
then user nhận được Job ID mà không cần chờ inference hoàn thành.**

### **AC-ARCH2-02**

**Given worker crash giữa generation,  
when lease hết hạn,  
then system có thể xác định job bị orphan.**

### **AC-ARCH2-03**

**Given transient infrastructure error,  
when retry policy cho phép,  
then job được retry nhưng không vượt maxAttempts.**

### **AC-ARCH2-04**

**Given frontend refresh browser,  
when user mở lại Scene,  
then running job status vẫn truy xuất được.**

### **AC-ARCH2-05**

**Given user chỉnh Character trong khi image generation đang chạy,  
then running job vẫn sử dụng context snapshot cũ.**

### **AC-ARCH2-06**

**Given user cancel job,  
then partial output không được trở thành selected candidate.**

### **AC-ARCH2-07**

**Given duplicate queue delivery,  
then system không tạo duplicate canonical result.**

### **AC-ARCH2-08**

**Given một AI worker mới được thêm vào worker pool,  
then Application API không cần thay đổi workflow.**

# **96. Locked MVP Runtime Stack**

**Implementation runtime của MVP được khóa như sau:**

### **Frontend**

**Vue 3 + Vite + TypeScript.**

### **Application Backend**

**ASP.NET Core.**

### **Database**

**PostgreSQL.**

### **Queue / Runtime Coordination**

**RabbitMQ.**

### **Realtime**

**SignalR optional. Realtime event chỉ hỗ trợ UX; Job state từ REST/database vẫn là source of truth.**

### **AI Worker**

**Python.**

### **AI Runtime**

**PyTorch / Diffusers / research implementation.**

### **Asset Storage**

**IAssetStorage abstraction.**

**Development sử dụng LocalFileAssetStorage. Demo / production-like sử dụng S3AssetStorage với S3-compatible provider, ví dụ Cloudflare R2.**

### **Development Orchestration**

**Docker Compose.**

# **97. MVP Simplification**

**Để luận văn không bị sa vào DevOps quá mức, MVP có thể đơn giản hóa:**

**1 Vue Frontend**

**1 ASP.NET Backend**

**1 PostgreSQL Database**

**1 RabbitMQ**

**1 Python AI Worker**

**1 Asset Storage implementation phù hợp environment**

**Không cần:**

- **Kubernetes;**

- **service mesh;**

- **multi-region;**

- **autoscaling cluster;**

- **distributed tracing platform phức tạp.**

**Kiến trúc vẫn phải giữ boundary để sau này mở rộng được.**

# **98. Runtime Flow Summary**

**Một generation hoàn chỉnh:**

**User clicks Generate**

**↓**

**API validates**

**↓**

**Context snapshot created**

**↓**

**Job persisted**

**↓**

**Queue message published**

**↓**

**Worker claims job**

**↓**

**AI generation**

**↓**

**Validation**

**↓**

**Asset persisted**

**↓**

**Candidate created**

**↓**

**Job completed**

**↓**

**Frontend notified**

**↓**

**User reviews candidate**

# **99. Failure Flow Summary**

**Worker / Provider Failure**

**↓**

**Classify Error**

**↓**

**Retryable?**

**┌────┴────┐**

**Yes No**

**↓ ↓**

**Retry FAILED**

**↓**

**Max attempts reached?**

**↓**

**FAILED**

**Nói bằng lời:**

**Hệ thống không đơn giản retry mọi lỗi. Trước tiên phải phân loại lỗi. Chỉ transient failure mới được retry. Nếu lỗi logic, input hoặc validation chắc chắn không thể tự sửa bằng chạy lại, job phải dừng.**

# **100. Deployment Principle**

**Nguyên tắc cuối cùng:**

> **Web application and AI runtime must be able to evolve independently.**

**OWNIVERSE phải có thể thay đổi:**

- **frontend;**

- **backend deployment;**

- **queue;**

- **GPU environment;**

- **AI model;**

**mà không phá vỡ toàn bộ hệ thống.**

**Product application giữ state và workflow.**

**AI runtime chỉ xử lý workload được giao.**

# **101. Architecture Group Summary**

**Nhóm 3 gồm hai tài liệu:**

**ARCH-01 — Solution Architecture Specification  
→ định nghĩa hệ thống gồm những thành phần nào và chúng chịu trách nhiệm gì.**

**ARCH-02 — Runtime, Job Orchestration & Deployment Specification  
→ định nghĩa những thành phần đó chạy thực tế như thế nào, đặc biệt đối với AI jobs và deployment.**

**Có thể nhớ:**

> **ARCH-01 = hệ thống được xây thành những khối nào.  
> ARCH-02 = những khối đó phối hợp và vận hành như thế nào khi hệ thống đang chạy.**
