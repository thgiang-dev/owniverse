# **API-01 — API Contract & Integration Specification**

**Document ID:** API-01  
**Document Type:** API Contract & Integration Specification  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Primary Role:** API Designer / Backend Architect  
**Status:** Draft v1  
**Parent Documents:** PROD-01, PROD-02, ARCH-01, ARCH-02, DOMAIN-01, DOMAIN-02, DATA-01, DATA-02  
**Related Documents:** API-02, AI-01, AI-02, AI-03, QA-01

## **1. Purpose**

Tài liệu này định nghĩa public application API của OWNIVERSE.

Nó trả lời:

> Frontend đọc và thay đổi Project, Story, Character, Scene và generated outputs bằng cách nào?

Tài liệu xác định:

- REST conventions;

- URL structure;

- JSend response format;

- request/response DTO;

- resource contract;

- authorization;

- validation;

- optimistic concurrency;

- pagination;

- idempotency;

- canonical mutation;

- generation request boundary;

- asset integration;

- error semantics.

# **2. Scope**

API-01 tập trung vào giao tiếp giữa:

Frontend

↓

Application API

↓

Application / Domain Layer

Bao gồm API cho:

- Project;

- Story;

- Story Core;

- World & Location;

- Character;

- Character Reference;

- Character Relationship;

- Scene;

- SceneCharacter;

- Story Fact;

- Generation request;

- Candidate;

- Asset.

Job lifecycle, realtime progress, worker callback, retry và asynchronous error contract được mô tả sâu hơn trong **API-02**.

# **3. Out of Scope**

API-01 không định nghĩa:

- database query trực tiếp;

- AI model API;

- Python model implementation;

- queue protocol chi tiết;

- SignalR event contract chi tiết;

- internal worker lifecycle;

- storage provider SDK.

# **4. API Style**

OWNIVERSE sử dụng:

> **REST-oriented HTTP API**

API tập trung vào resource và business action rõ ràng.

Ví dụ:

GET /api/v1/characters/{characterId}

hoặc:

POST /api/v1/scenes/{sceneId}/candidates/{candidateId}/select

Không sử dụng các endpoint generic như:

POST /api/update-data

# **5. Base URL**

Toàn bộ API được đặt dưới:

/api/v1

Ví dụ:

GET /api/v1/projects

Việc có /v1 từ đầu giúp API có boundary rõ ràng khi contract thay đổi lớn sau này.

# **6. Content Type**

Request và response mặc định:

Content-Type: application/json

Accept: application/json

Ngoại trừ file upload/download flow đặc biệt.

# **7. JSON Naming Convention**

JSON sử dụng:

> **camelCase**

Ví dụ:

{

"characterId": "...",

"stableIdentity": {},

"createdAt": "..."

}

Không sử dụng database naming như:

{

"character_id": "..."

}

trong public API.

# **8. Identifier**

Public resource identifier sử dụng UUID.

Ví dụ:

{

"characterId": "2a1a5703-f8b1-436d-9c78-e47896055ab9"

}

Client không được suy luận quan hệ từ giá trị UUID.

# **9. Date and Time**

Datetime sử dụng ISO 8601.

API ưu tiên UTC:

{

"createdAt": "2026-09-13T15:30:00Z"

}

Frontend chịu trách nhiệm chuyển sang timezone hiển thị của user.

# **10. Standard Response — JSend**

OWNIVERSE áp dụng **JSend** cho tất cả HTTP responses.

JSend có ba trạng thái:

success

fail

error

# **11. JSend Success**

Operation thành công:

{

"status": "success",

"data": {

"project": {

"projectId": "...",

"title": "My Story"

}

}

}

Nếu operation thành công nhưng không cần trả resource:

{

"status": "success",

"data": null

}

OWNIVERSE vì vậy không cần sử dụng HTTP 204 No Content cho các public API thông thường.

# **12. JSend Fail**

fail được dùng khi request không thể thực hiện do:

- input không hợp lệ;

- resource không tồn tại;

- không có quyền;

- revision conflict;

- business rule;

- rate limit;

- trạng thái resource không cho phép operation.

Ví dụ:

{

"status": "fail",

"data": {

"errorKey": "REVISION_CONFLICT",

"message": "The character has been modified.",

"expectedRevision": 4,

"currentRevision": 5

}

}

# **13. Validation Fail**

Ví dụ:

{

"status": "fail",

"data": {

"errorKey": "VALIDATION_FAILED",

"message": "The request contains invalid fields.",

"fields": {

"name": \[

"Character name is required."

\]

}

}

}

# **14. JSend Error**

error dành cho lỗi hệ thống hoặc infrastructure.

JSend error bắt buộc có:

status

message

Có thể có numeric code và data.

Ví dụ:

{

"status": "error",

"message": "An unexpected server error occurred.",

"code": 50001,

"data": {

"errorKey": "INTERNAL_SERVER_ERROR",

"traceId": "00-f329..."

}

}

# **15. HTTP Status + JSend Mapping**

| **HTTP** | **JSend** | **Ý nghĩa**                    |
|----------|-----------|--------------------------------|
| 200      | success   | Operation thành công           |
| 201      | success   | Resource được tạo              |
| 202      | success   | Async operation được chấp nhận |
| 400      | fail      | Request/input sai              |
| 401      | fail      | Chưa xác thực                  |
| 403      | fail      | Không có quyền                 |
| 404      | fail      | Resource không tồn tại         |
| 409      | fail      | Conflict, thường revision      |
| 422      | fail      | Business rule không cho phép   |
| 429      | fail      | Rate limit                     |
| 500      | error     | Internal server error          |
| 502      | error     | External dependency lỗi        |
| 503      | error     | Service tạm thời unavailable   |
| 504      | error     | Dependency timeout             |

Nguyên tắc:

> **4xx không phải server crash → JSend fail.  
> 5xx là system/infrastructure failure → JSend error.**

# **16. Authentication**

Protected endpoints yêu cầu:

Authorization: Bearer {accessToken}

Authentication mechanism cụ thể có thể là internal hoặc external identity provider.

API contract không phụ thuộc provider.

# **17. Authorization**

Backend luôn kiểm tra ownership.

Ví dụ request:

GET /api/v1/characters/{characterId}

không chỉ kiểm tra Character tồn tại.

Backend phải xác định:

Character

↓

Story

↓

Project

↓

Owner

User phải có quyền với Project đó.

# **18. Do Not Trust Resource IDs**

Biết UUID không đồng nghĩa có quyền truy cập resource.

Rule:

**API-AUTH-01**

> Every protected resource operation SHALL validate ownership or granted access on the server.

Frontend ẩn button không được xem là authorization.

# **19. Optimistic Concurrency**

Canonical resources sử dụng revision.

Ví dụ GET Character trả:

{

"status": "success",

"data": {

"character": {

"characterId": "...",

"name": "Aira",

"revision": 4

}

}

}

Client khi update gửi:

{

"expectedRevision": 4,

"name": "Aira"

}

# **20. Revision Conflict**

Nếu current revision đã là 5:

409 Conflict

{

"status": "fail",

"data": {

"errorKey": "REVISION_CONFLICT",

"message": "This resource has changed since it was loaded.",

"expectedRevision": 4,

"currentRevision": 5

}

}

Client phải refresh hoặc resolve conflict.

# **21. Pagination**

Các collection lớn sử dụng:

page

pageSize

Ví dụ:

GET /api/v1/projects?page=1&pageSize=20

Response:

{

"status": "success",

"data": {

"items": \[\],

"pagination": {

"page": 1,

"pageSize": 20,

"totalItems": 54,

"totalPages": 3

}

}

}

# **22. Pagination Limits**

Recommended:

default pageSize = 20

maximum pageSize = 100

Exact limits có thể config.

# **23. Sorting**

Collection có thể hỗ trợ:

sort

order

Ví dụ:

GET /api/v1/projects?sort=updatedAt&order=desc

Chỉ whitelist các field được phép sort.

# **24. Filtering**

Filtering được thiết kế theo resource.

Ví dụ:

GET /api/v1/scenes?storyId={id}&status=DRAFT

Không xây một query language generic cho MVP.

# **25. Idempotency**

Các POST có khả năng gây duplicate workload, đặc biệt generation, hỗ trợ:

Idempotency-Key: {client-generated-key}

Nếu request giống nhau được retry do network error, backend có thể trả operation đã được tạo trước đó.

# **26. Correlation / Trace ID**

Server nên đưa traceId vào error response.

Có thể đồng thời trả:

X-Trace-Id

để hỗ trợ debugging.

# **27. Main API Groups**

Public API được chia thành:

Projects

Stories

Story Core

World / Locations

Characters

Character References

Relationships

Scenes

Scene Characters

Story Facts

Generations

Jobs

Candidates

Assets

# **28. Project API**

## **Create Project**

POST /api/v1/projects

Request:

{

"title": "The Last Memory",

"description": "A story about a girl searching for her forgotten past."

}

Success:

201 Created

{

"status": "success",

"data": {

"project": {

"projectId": "84dcd850-f209-47f6-b29e-f187fe4d96aa",

"title": "The Last Memory",

"description": "A story about a girl searching for her forgotten past.",

"status": "DRAFT",

"revision": 1,

"createdAt": "2026-09-13T15:30:00Z"

}

}

}

# **29. List Projects**

GET /api/v1/projects

Optional:

page

pageSize

status

sort

order

Response trả collection + pagination.

# **30. Get Project**

GET /api/v1/projects/{projectId}

Response:

{

"status": "success",

"data": {

"project": {

"projectId": "...",

"title": "The Last Memory",

"status": "ACTIVE",

"revision": 4

}

}

}

# **31. Update Project**

PATCH /api/v1/projects/{projectId}

Request:

{

"expectedRevision": 4,

"title": "The Last Memory",

"description": "Updated description."

}

Only provided mutable fields are changed.

# **32. Archive Project**

POST /api/v1/projects/{projectId}/archive

Request:

{

"expectedRevision": 4

}

Archive được ưu tiên hơn destructive DELETE.

# **33. Story API**

Trong MVP, Project có một primary Story.

Lấy Story:

GET /api/v1/projects/{projectId}/story

# **34. Initialize Story**

Nếu Story chưa tồn tại:

POST /api/v1/projects/{projectId}/story

Request:

{

"title": "The Last Memory",

"language": "en",

"format": "ILLUSTRATED_STORY",

"idea": "A girl searches for the memories she lost."

}

Backend có thể đồng thời tạo empty Story Core container.

# **35. Update Story**

PATCH /api/v1/stories/{storyId}

Request:

{

"expectedRevision": 2,

"title": "The Last Memory",

"format": "ILLUSTRATED_STORY"

}

# **36. Story Core API**

Lấy Story Core:

GET /api/v1/stories/{storyId}/core

Response:

{

"status": "success",

"data": {

"storyCore": {

"storyCoreId": "...",

"premise": "A girl searches for her forgotten past.",

"synopsis": "...",

"theme": "Identity and belonging",

"tone": "Melancholic but hopeful",

"creativeDirection": {

"visualStyle": "cinematic anime fantasy",

"pacing": "slow emotional"

},

"revision": 6

}

}

}

# **37. Update Story Core**

PATCH /api/v1/stories/{storyId}/core

Request:

{

"expectedRevision": 6,

"premise": "...",

"theme": "Identity and belonging",

"tone": "Melancholic but hopeful",

"creativeDirection": {

"visualStyle": "cinematic anime fantasy"

}

}

Canonical Story Core update tạo new version theo DATA-02.

# **38. World API**

Get World:

GET /api/v1/stories/{storyId}/world

Create/update có thể dùng:

PUT /api/v1/stories/{storyId}/world

PUT phù hợp vì MVP có một World resource chính cho Story.

# **39. Location API**

List:

GET /api/v1/stories/{storyId}/locations

Create:

POST /api/v1/stories/{storyId}/locations

Update:

PATCH /api/v1/locations/{locationId}

Archive:

POST /api/v1/locations/{locationId}/archive

# **40. Character API**

List Characters:

GET /api/v1/stories/{storyId}/characters

Có thể filter:

status

role

search

# **41. Create Character**

POST /api/v1/stories/{storyId}/characters

Request:

{

"name": "Aira",

"narrativeRole": "PROTAGONIST",

"shortBiography": "A young traveler searching for her memories.",

"stableIdentity": {

"hairColor": "black",

"eyeColor": "blue",

"distinctiveMarks": \[

"scar under left eye"

\]

},

"baseAppearance": {

"hairStyle": "long",

"defaultOutfit": "white shirt and dark skirt"

},

"personality": {

"traits": \[

"quiet",

"curious"

\]

}

}

# **42. Character Response**

{

"status": "success",

"data": {

"character": {

"characterId": "...",

"storyId": "...",

"name": "Aira",

"narrativeRole": "PROTAGONIST",

"stableIdentity": {

"hairColor": "black",

"eyeColor": "blue",

"distinctiveMarks": \[

"scar under left eye"

\]

},

"baseAppearance": {

"hairStyle": "long"

},

"revision": 1

}

}

}

# **43. Get Character**

GET /api/v1/characters/{characterId}

# **44. Update Character**

PATCH /api/v1/characters/{characterId}

Request:

{

"expectedRevision": 4,

"shortBiography": "Updated biography.",

"baseAppearance": {

"hairStyle": "long",

"defaultOutfit": "black travel coat"

}

}

Locked attributes không được thay đổi nếu workflow chưa unlock.

# **45. Locked Attribute Failure**

422 Unprocessable Entity

{

"status": "fail",

"data": {

"errorKey": "CHARACTER_ATTRIBUTE_LOCKED",

"message": "One or more character attributes are locked.",

"attributes": \[

"stableIdentity.eyeColor"

\]

}

}

# **46. Archive Character**

POST /api/v1/characters/{characterId}/archive

Không hard-delete Character đã có Scene hoặc generation history.

# **47. Character Lock API**

Lock attribute:

PUT /api/v1/characters/{characterId}/locks/{attributePath}

Ví dụ encoded path tương ứng:

stableIdentity.eyeColor

Request:

{

"locked": true

}

# **48. Character Reference API**

List:

GET /api/v1/characters/{characterId}/references

Attach Asset:

POST /api/v1/characters/{characterId}/references

Request:

{

"assetId": "...",

"referenceType": "UPLOADED",

"isPrimary": true

}

# **49. Set Primary Character Reference**

POST /api/v1/characters/{characterId}/references/{referenceId}/make-primary

Backend đảm bảo primary reference rule.

# **50. Archive Character Reference**

POST /api/v1/characters/{characterId}/references/{referenceId}/archive

Asset không mặc định bị xóa.

# **51. Character Relationship API**

Create:

POST /api/v1/stories/{storyId}/relationships

Request:

{

"sourceCharacterId": "...",

"targetCharacterId": "...",

"relationshipType": "FRIEND",

"directionality": "BIDIRECTIONAL",

"description": "Childhood friends."

}

Backend phải xác nhận cả hai Character thuộc cùng Story.

# **52. Scene API**

List:

GET /api/v1/stories/{storyId}/scenes

Default sort:

sceneOrder ASC

# **53. Create Scene**

POST /api/v1/stories/{storyId}/scenes

Request:

{

"title": "Arrival at the Capital",

"summary": "Aira reaches the capital during heavy rain.",

"description": "Aira enters through the northern gate.",

"primaryLocationId": "...",

"narrativePurpose": "Introduce the capital.",

"emotionalBeat": "Isolation and anticipation"

}

Backend xác định sceneOrder hoặc nhận explicit placement instruction.

# **54. Get Scene**

GET /api/v1/scenes/{sceneId}

Response có thể aggregate dữ liệu cần cho Scene Workspace:

{

"status": "success",

"data": {

"scene": {

"sceneId": "...",

"title": "Arrival at the Capital",

"sceneOrder": 120,

"description": "Aira enters through the northern gate.",

"location": {

"locationId": "...",

"name": "Royal Capital"

},

"characters": \[

{

"characterId": "...",

"name": "Aira",

"state": {

"outfit": "black travel coat",

"emotion": "exhausted",

"physicalCondition": {

"wetFromRain": true

},

"action": "walking through the city gate"

}

}

\],

"selectedCandidateId": null,

"revision": 5

}

}

}

# **55. Update Scene**

PATCH /api/v1/scenes/{sceneId}

Request:

{

"expectedRevision": 5,

"description": "...",

"primaryLocationId": "...",

"emotionalBeat": "Fear"

}

# **56. Reorder Scenes**

PUT /api/v1/stories/{storyId}/scene-order

Request:

{

"scenes": \[

{

"sceneId": "...",

"order": 10

},

{

"sceneId": "...",

"order": 20

},

{

"sceneId": "...",

"order": 30

}

\]

}

Reordering có thể trigger continuity review theo DOMAIN-02.

# **57. Archive Scene**

POST /api/v1/scenes/{sceneId}/archive

Historical candidates không bị xóa.

# **58. SceneCharacter API**

Assign hoặc update Character state:

PUT /api/v1/scenes/{sceneId}/characters/{characterId}

Request:

{

"sceneRole": "PRIMARY",

"outfit": {

"description": "black travel coat"

},

"appearanceOverride": {},

"emotion": "exhausted",

"action": "walking through the city gate",

"physicalCondition": {

"wetFromRain": true

}

}

# **59. SceneCharacter Rule**

Endpoint trên không thay đổi:

Character.stableIdentity

Nó chỉ cập nhật:

SceneCharacter state

# **60. Remove Character From Scene**

DELETE /api/v1/scenes/{sceneId}/characters/{characterId}

Vì ta sử dụng JSend nhất quán:

{

"status": "success",

"data": null

}

thay vì HTTP 204.

# **61. Story Fact API**

List Story Facts:

GET /api/v1/stories/{storyId}/facts

Có thể filter:

status

scope

sceneId

characterId

# **62. Create Canonical Story Fact**

POST /api/v1/stories/{storyId}/facts

Request:

{

"factText": "Aira knows that Ren is the masked knight.",

"scope": "CHARACTER",

"effectiveFromSceneId": "...",

"entities": \[

{

"entityType": "CHARACTER",

"entityId": "...",

"role": "SUBJECT"

}

\]

}

# **63. Generation API**

Generation là bridge giữa API-01 và API-02.

Request Scene image generation:

POST /api/v1/scenes/{sceneId}/generations

# **64. Generation Request**

{

"operationType": "IMAGE_GENERATION",

"candidateCount": 4,

"instruction": "Emphasize the rainy cinematic atmosphere."

}

Frontend **không cần và không được xem là nguồn xây technical model prompt**.

Backend/AI Application Layer xây GenerationContextV1, đóng băng thành Context Snapshot, sau đó Adapter-specific Prompt/Condition Builder chuyển context đó thành model-specific input.

# **65. Generation Accepted Response**

Generation là asynchronous nên trả:

202 Accepted

{

"status": "success",

"data": {

"job": {

"jobId": "8eef1312-7295-4558-a983-45d817342c7b",

"operationType": "IMAGE_GENERATION",

"status": "QUEUED",

"target": {

"type": "SCENE",

"id": "..."

},

"createdAt": "2026-09-13T15:45:00Z"

}

}

}

Job lifecycle chi tiết thuộc API-02.

# **66. Regeneration**

Regeneration vẫn sử dụng generation resource:

POST /api/v1/scenes/{sceneId}/generations

Request có thể thêm:

{

"operationType": "IMAGE_REGENERATION",

"sourceCandidateId": "...",

"candidateCount": 2,

"instruction": "Keep the composition but make the expression more determined."

}

Regeneration luôn tạo Job/Candidate mới.

# **67. Job API Boundary**

Frontend có thể query:

GET /api/v1/jobs/{jobId}

Response format và toàn bộ Job State contract được khóa trong API-02.

# **68. Candidate API**

List candidates của Scene:

GET /api/v1/scenes/{sceneId}/candidates

Có thể filter:

status

contentType

# **69. Candidate Response**

{

"status": "success",

"data": {

"items": \[

{

"candidateId": "...",

"contentType": "IMAGE",

"status": "VALID",

"asset": {

"assetId": "...",

"width": 1024,

"height": 1024

},

"validation": {

"decision": "ACCEPT",

"consistencyStatus": "PASS"

},

"createdAt": "..."

}

\],

"pagination": {

"page": 1,

"pageSize": 20,

"totalItems": 4,

"totalPages": 1

}

}

}

# **70. Select Candidate**

POST /api/v1/scenes/{sceneId}/candidates/{candidateId}/select

Request:

{

"expectedSceneRevision": 7

}

Backend xác nhận:

Candidate targets this Scene

Candidate is selectable

User owns Project

Scene revision matches

# **71. Selection Response**

{

"status": "success",

"data": {

"scene": {

"sceneId": "...",

"selectedCandidateId": "...",

"revision": 8

}

}

}

Candidate không bị overwrite.

# **72. Stale Candidate**

Candidate response có thể chứa:

{

"stale": {

"status": "REVIEW_RECOMMENDED",

"message": "Generated with older character data."

}

}

Không expose raw dependency graph mặc định trong creative UI.

# **73. Asset API**

Asset metadata:

GET /api/v1/assets/{assetId}

# **74. Asset Access**

Để lấy file private:

POST /api/v1/assets/{assetId}/access

Backend:

Validate ownership

↓

Generate temporary access URL

Success:

{

"status": "success",

"data": {

"url": "https://...",

"expiresAt": "2026-09-13T15:55:00Z"

}

}

# **75. Asset Upload Request**

User muốn upload Character Reference:

POST /api/v1/projects/{projectId}/assets/upload-requests

Request:

{

"assetType": "CHARACTER_REFERENCE",

"fileName": "aira-reference.png",

"mimeType": "image/png",

"fileSize": 1842932

}

# **76. Upload Response**

{

"status": "success",

"data": {

"assetId": "...",

"upload": {

"url": "https://...",

"expiresAt": "..."

}

}

}

Sau upload, client có thể confirm theo storage strategy.

# **77. API Does Not Expose Storage Credentials**

Frontend chỉ nhận temporary upload/access URL.

Không bao giờ nhận:

- S3 secret;

- storage account key;

- backend credentials.

# **78. Canonical Mutation Rule**

Các API generated content không được tự động chỉnh:

Story Core

Character

Scene Canon

Story Facts

Ví dụ AI đề xuất synopsis phải đi qua:

Proposal / Candidate

↓

Explicit Accept

↓

Canonical Update Command

# **79. AI Proposal Acceptance**

Ví dụ:

POST /api/v1/ai-proposals/{proposalId}/accept

Backend thực hiện:

Validate proposal

↓

Validate target revision

↓

Perform canonical command

↓

Create new version

↓

Mark proposal ACCEPTED

# **80. Generic Patch Restriction**

Không có API cho phép client gửi:

{

"table": "characters",

"field": "..."

}

API contract luôn dựa trên domain/resource.

# **81. Validation**

Validation gồm hai tầng.

**Structural validation**

Ví dụ:

required

string length

valid UUID

enum

**Business validation**

Ví dụ:

Scene and Character belong to same Story

Candidate belongs to Scene

Attribute is not locked

# **82. Validation Failure Example**

400 Bad Request

{

"status": "fail",

"data": {

"errorKey": "VALIDATION_FAILED",

"message": "The request contains invalid fields.",

"fields": {

"candidateCount": \[

"Candidate count must be between 1 and 4."

\]

}

}

}

# **83. Business Rule Failure**

422 Unprocessable Entity

{

"status": "fail",

"data": {

"errorKey": "CHARACTER_NOT_IN_STORY",

"message": "The selected character does not belong to this story."

}

}

# **84. Resource Not Found**

404 Not Found

{

"status": "fail",

"data": {

"errorKey": "CHARACTER_NOT_FOUND",

"message": "The requested character was not found."

}

}

For private resources, implementation may intentionally return 404 instead of revealing that a resource exists but belongs to someone else.

# **85. Unauthorized**

401 Unauthorized

{

"status": "fail",

"data": {

"errorKey": "AUTHENTICATION_REQUIRED",

"message": "Authentication is required."

}

}

# **86. Forbidden**

403 Forbidden

{

"status": "fail",

"data": {

"errorKey": "ACCESS_DENIED",

"message": "You do not have permission to perform this action."

}

}

# **87. Rate Limited**

429 Too Many Requests

{

"status": "fail",

"data": {

"errorKey": "RATE_LIMIT_EXCEEDED",

"message": "Too many generation requests.",

"retryAfterSeconds": 30

}

}

# **88. System Error**

500 Internal Server Error

{

"status": "error",

"message": "An unexpected error occurred.",

"code": 50001,

"data": {

"errorKey": "INTERNAL_SERVER_ERROR",

"traceId": "..."

}

}

Raw stack trace không được trả cho client production.

# **89. Dependency Failure**

Ví dụ Asset Storage unavailable:

503 Service Unavailable

{

"status": "error",

"message": "Asset storage is temporarily unavailable.",

"code": 50301,

"data": {

"errorKey": "ASSET_STORAGE_UNAVAILABLE",

"traceId": "..."

}

}

# **90. Error Key Naming**

Machine-readable errorKey sử dụng:

UPPER_SNAKE_CASE

Ví dụ:

PROJECT_NOT_FOUND

REVISION_CONFLICT

CHARACTER_ATTRIBUTE_LOCKED

GENERATION_LIMIT_REACHED

Frontend không nên parse message để xác định logic.

# **91. User-Facing Message**

API message phải đủ dễ hiểu nhưng frontend vẫn có thể localize bằng errorKey.

Ví dụ frontend nhận:

REVISION_CONFLICT

và hiển thị localized Vietnamese message.

# **92. Request DTO Principle**

Public Request DTO không phải database entity.

Ví dụ Character database có:

created_at

revision_number

story_id

nhưng CreateCharacterRequest không cho client tự đặt các field internal này.

# **93. Response DTO Principle**

API không expose internal persistence fields không cần thiết.

Ví dụ:

storage_bucket

storage_key

internal_worker_id

không được trả trong public resource DTO.

# **94. Partial Updates**

PATCH chỉ thay các field được gửi.

Không gửi:

{

"shortBiography": null

}

trừ khi client thực sự muốn clear field đó và API cho phép.

Missing field và explicit null là hai ý nghĩa khác nhau.

# **95. Collection Embedding**

Endpoint chi tiết có thể embed dữ liệu nhỏ hữu ích.

Ví dụ Scene GET có thể trả Character name/state.

Nhưng không embed:

all Character versions

all generation jobs

all candidates

all story history

vào một request duy nhất.

# **96. API Aggregation for UX**

REST resource design không có nghĩa frontend phải thực hiện 20 requests cho một màn hình.

Có thể tạo read endpoint phù hợp với UX.

Ví dụ:

GET /api/v1/scenes/{sceneId}

có thể trả aggregate đủ cho Scene Workspace:

- Scene;

- location summary;

- Character states;

- selected Candidate summary.

Đây vẫn là application-level contract hợp lệ.

# **97. Internal vs Public API**

Public frontend API:

/api/v1/...

AI Worker không được giả làm frontend và sử dụng toàn bộ API này.

Internal worker integration sẽ được mô tả trong API-02.

# **98. Generation Boundary**

Public API chỉ cần nói:

Create Job

Query Job

Cancel Job

Read Result

Không expose:

claim queue message

set worker heartbeat

directly mark Job complete

cho frontend.

# **99. API Security Requirements**

**API-SEC-01  
**All protected resources SHALL verify authenticated user.

**API-SEC-02  
**Resource ownership SHALL be validated server-side.

**API-SEC-03  
**Model/storage secrets SHALL never appear in client responses.

**API-SEC-04  
**Request payload SHALL be validated before business execution.

**API-SEC-05  
**Raw exception data SHALL not be exposed to production clients.

# **100. API Functional Requirements**

**API-FR-01  
**All HTTP API responses SHALL conform to the OWNIVERSE JSend convention.

**API-FR-02  
**All successful responses SHALL use status = "success".

**API-FR-03  
**All normal 4xx request/business failures SHALL use status = "fail".

**API-FR-04  
**All system-level 5xx failures SHALL use status = "error".

**API-FR-05  
**Canonical updates SHALL support revision-based concurrency validation where applicable.

**API-FR-06  
**API SHALL verify resource ownership.

**API-FR-07  
**Generated Candidate SHALL not automatically mutate Canon.

**API-FR-08  
**Long-running generation SHALL return a Job rather than wait for final output.

**API-FR-09  
**Collections with potentially large result sets SHALL support pagination.

**API-FR-10  
**Frontend SHALL not require knowledge of database schema.

**API-FR-11  
**Frontend SHALL not require knowledge of AI model-specific prompt syntax.

**API-FR-12  
**Private asset access SHALL be authorization-controlled.

# **101. Acceptance Criteria**

### **AC-API-01**

Given a successful Project request,  
when the server responds,  
then response SHALL contain:

{

"status": "success",

"data": {}

}

### **AC-API-02**

Given invalid Character input,  
when validation fails,  
then response SHALL use an HTTP 4xx code and JSend fail.

### **AC-API-03**

Given an unexpected database failure,  
when request processing fails,  
then response SHALL use a 5xx code and JSend error.

### **AC-API-04**

Given Character revision 5,  
when client tries to update using expected revision 4,  
then API SHALL reject the update with REVISION_CONFLICT.

### **AC-API-05**

Given Character belongs to another user's Project,  
when a user requests it,  
then API SHALL not expose the Character.

### **AC-API-06**

Given user requests Scene generation,  
when request is accepted,  
then API SHALL return HTTP 202 with a Job identifier without waiting for inference.

### **AC-API-07**

Given Candidate A belongs to Scene 1,  
when client attempts to select it for Scene 2,  
then API SHALL reject the operation.

### **AC-API-08**

Given AI produces a new Candidate,  
then API SHALL NOT automatically modify canonical Scene content.

### **AC-API-09**

Given SceneCharacter is updated,  
then canonical Character Stable Identity SHALL remain unchanged.

### **AC-API-10**

Given private Asset belongs to Project A,  
when unauthorized user requests access,  
then API SHALL not return an access URL.

# **102. MVP API Surface**

Phiên bản đầu không cần triển khai tất cả endpoint của tài liệu.

Core MVP nên ưu tiên:

Projects

Story

Story Core

Characters

Character References

Scenes

Scene Characters

Generation

Job Status

Candidates

Assets

Có thể bổ sung sau:

World

Locations

Relationships

Story Facts

Knowledge

Events

Version History

AI Proposals

# **103. Core End-to-End API Flow**

Một flow chính:

POST /projects

↓

POST /projects/{id}/story

↓

PATCH /stories/{id}/core

↓

POST /stories/{id}/characters

↓

POST /stories/{id}/scenes

↓

PUT /scenes/{id}/characters/{characterId}

↓

POST /scenes/{id}/generations

↓

GET /jobs/{jobId}

↓

GET /scenes/{id}/candidates

↓

POST /scenes/{id}/candidates/{candidateId}/select

Giải thích bằng lời:

Frontend tạo Project và Story, thiết lập Story Core, thêm Character và Scene, gán Character vào Scene rồi yêu cầu AI generation. Backend trả Job ID. Khi Job hoàn tất, frontend lấy Candidates và user chọn kết quả muốn sử dụng.

# **104. Traceability**

PROD-02

User Workflow

↓

DOMAIN

Resources / Business Rules

↓

DATA

Persistence

↓

API-01

HTTP Contracts

↓

Frontend

Ví dụ:

PROD:

Select generated output

↓

DOMAIN:

Selected Candidate

↓

DATA:

scenes.selected_candidate_id

↓

API:

POST /scenes/{sceneId}/candidates/{candidateId}/select

# **105. JSend Decision**

Đây là architectural API decision của OWNIVERSE:

> **JSend is the normative HTTP response envelope for API-01 and API-02.**

Cụ thể:

2xx → success

4xx → fail

5xx → error

Không tạo nhiều response format khác nhau giữa các module.

# **106. Boundary With API-02**

API-01 đã định nghĩa:

> user bắt đầu generation bằng cách nào.

API-02 sẽ tiếp tục từ:

Job created

và định nghĩa:

QUEUED

↓

RUNNING

↓

VALIDATING

↓

COMPLETED / FAILED

cùng với:

- polling;

- SignalR events;

- cancellation;

- retry;

- idempotency;

- worker contract;

- async error;

- stale notification.

Các HTTP response trong API-02 **vẫn dùng chính JSend convention được khóa tại API-01**.

Realtime events sẽ có event envelope riêng vì bản chất của chúng không phải HTTP response.

# **107. Core Principle**

Nguyên tắc cuối cùng của API-01:

> **The API exposes product intent, not database structure or AI machinery.**

Nói đơn giản:

Frontend nên nói:

> **“Tôi muốn tạo Scene.”  
> “Tôi muốn cập nhật Character.”  
> “Tôi muốn generate ảnh.”  
> “Tôi chọn Candidate này.”**

chứ không phải:

> **“Hãy sửa table này.”  
> “Hãy gọi model này với prompt này.”**

Backend chịu trách nhiệm biến ý định đó thành domain operation đúng và an toàn.
