# **ARCH-01 — Solution Architecture Specification**

**Document ID:** ARCH-01  
**Document Type:** Solution Architecture Specification  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Primary Role:** Solution Architect  
**Status:** Draft v1  
**Parent Documents:** PROD-01, PROD-02, UX-01, UX-02, UX-03  
**Related Documents:** ARCH-02, DATA-01, API-01, AI-01, AI-02, AI-03

# **1. Purpose**

Tài liệu này xác định kiến trúc tổng thể của OWNIVERSE ở cấp hệ thống.

Mục tiêu là trả lời:

- hệ thống được chia thành những thành phần nào;

- mỗi thành phần chịu trách nhiệm gì;

- dữ liệu đi qua hệ thống như thế nào;

- frontend giao tiếp với backend ra sao;

- backend điều phối AI như thế nào;

- generation jobs được tách khỏi request thông thường ra sao;

- canonical story state được bảo vệ như thế nào;

- asset, image và generation history được quản lý ở đâu;

- kiến trúc có thể mở rộng hoặc thay đổi AI model như thế nào.

Tài liệu không khóa implementation vào một cloud provider hoặc deployment platform cụ thể.

# **2. Architecture Goals**

## **ARCH-G01 — Clear Separation of Concerns**

Mỗi subsystem phải có trách nhiệm rõ ràng.

Frontend không trực tiếp gọi AI model.

AI model không trực tiếp chỉnh database canonical.

Storage không chứa business logic.

## **ARCH-G02 — Canonical Story Protection**

Story, Character và Scene canonical state phải được quản lý bởi application backend.

AI chỉ cung cấp proposal hoặc generated output.

## **ARCH-G03 — Asynchronous AI Processing**

Các AI task có thời gian chạy dài phải được thực hiện ngoài HTTP request thông thường.

Generation không được khóa application.

## **ARCH-G04 — Replaceable AI Services**

Hệ thống phải cho phép thay đổi:

- model;

- inference service;

- provider;

- research implementation;

mà không yêu cầu viết lại toàn bộ application.

## **ARCH-G05 — Traceable Generation**

Mỗi generated output phải truy ngược được tới:

- project;

- story;

- scene;

- context snapshot;

- generation job;

- model configuration.

## **ARCH-G06 — Failure Isolation**

AI hoặc worker lỗi không được làm hỏng canonical Story state.

# **3. Architectural Style**

OWNIVERSE sử dụng kiến trúc tổng thể theo hướng:

> **Modular Web Application + Asynchronous AI Processing**

Không cần bắt đầu bằng microservices hoàn toàn.

Phiên bản luận văn có thể triển khai backend dưới dạng một **modular monolith** — tức một backend application duy nhất nhưng được chia thành các module rõ ràng.

Các workload AI nặng được tách ra thành worker/service riêng.

# **4. Why Modular Monolith for MVP**

Microservices làm tăng:

- deployment complexity;

- network communication;

- distributed transactions;

- monitoring complexity;

- infrastructure requirements.

Trong khi MVP luận văn chưa cần scale ở mức đó.

Do đó architecture ban đầu ưu tiên:

One Application Backend

\+

Separate AI Workers

\+

External Storage / Database

Sau này các module có thể được tách thành service độc lập nếu cần.

# **5. High-Level Architecture**

User Browser

↓

Frontend Application

↓

Application API

↓

Application Backend

┌────────┬──────────┬──────────┬───────────┐

│ Story │Character │ Scene │Generation │

│ Module │ Module │ Module │ Module │

└────────┴──────────┴──────────┴───────────┘

↓

Job / Queue Layer

↓

AI Workers

↓

AI Models / Providers

Application Backend

↓ ↓

Database Asset Storage

Giải thích bằng lời:

Người dùng chỉ giao tiếp với frontend. Frontend gửi yêu cầu tới Application API. Backend quản lý toàn bộ dữ liệu Story, Character, Scene và Generation.

Nếu là thao tác nhanh như sửa tên character hoặc cập nhật scene, backend xử lý trực tiếp.

Nếu là tác vụ nặng như generate image, backend tạo job và đưa công việc sang worker. Worker mới là thành phần gọi AI model. Output sau đó quay trở lại application layer để được validate và lưu.

# **6. Main System Layers**

Hệ thống được chia thành sáu lớp logic:

1.  Presentation Layer;

2.  Application Layer;

3.  Domain Layer;

4.  AI Application Layer;

5.  Infrastructure Layer;

6.  Persistence & Storage Layer.

# **7. Presentation Layer**

Presentation Layer gồm:

- Web Frontend;

- browser UI;

- user interaction;

- client-side state;

- API consumption.

Frontend chịu trách nhiệm:

- render UI;

- collect user input;

- show AI job progress;

- show candidates;

- support editing;

- handle navigation.

Frontend không chứa canonical business rules quan trọng.

# **8. Application Layer**

Application Layer điều phối use case.

Ví dụ:

- Create Project;

- Create Character;

- Update Scene;

- Request Image Generation;

- Select Candidate;

- Regenerate Scene.

Application Layer chịu trách nhiệm:

- authorization;

- use-case orchestration;

- transaction boundaries;

- validation;

- domain interaction;

- job creation;

- event publishing.

# **9. Domain Layer**

Domain Layer chứa logic cốt lõi của sản phẩm.

Các aggregate/entity chính:

- Project;

- Story;

- Character;

- Scene;

- Character Reference;

- Generation Job;

- Candidate;

- Asset.

Domain Layer không phụ thuộc trực tiếp vào:

- HTTP;

- database engine;

- cloud storage;

- AI provider.

# **10. AI Application Layer**

AI Application Layer là abstraction giữa product backend và AI implementation.

Bao gồm:

- AI Orchestrator;

- Context Builder;

- Memory Manager;

- Prompt Builder;

- Generation Adapter;

- Validation Pipeline;

- Output Normalizer.

Ba tài liệu AI-01, AI-02, AI-03 mô tả chi tiết layer này.

# **11. Infrastructure Layer**

Infrastructure cung cấp adapter cho các thành phần bên ngoài.

Ví dụ:

- database access;

- file storage;

- message queue;

- AI provider API;

- local inference;

- cache;

- notification;

- observability.

# **12. Persistence & Storage Layer**

Persistence được chia thành ít nhất hai loại:

### **Structured Data**

Lưu bằng relational database.

Ví dụ:

- Project;

- Story;

- Character;

- Scene;

- Generation metadata.

### **Binary Assets**

Lưu bằng object/file storage.

Ví dụ:

- generated image;

- uploaded character reference;

- thumbnail;

- preview.

Database chỉ giữ asset reference và metadata.

# **13. Core Modules**

Backend được chia thành các module sau:

Project Module

Story Module

Character Module

Scene Module

Generation Module

Asset Module

AI Integration Module

Identity / Access Module

# **14. Project Module**

Trách nhiệm:

- create project;

- update project;

- archive project;

- project ownership;

- project lifecycle.

Project là boundary cao nhất của creative workspace.

# **15. Story Module**

Trách nhiệm:

- Story Core;

- premise;

- synopsis;

- world facts;

- relationships;

- style information;

- canonical story facts;

- version tracking.

Story Module là nguồn canonical cho story-level data.

# **16. Character Module**

Trách nhiệm:

- Character entity;

- stable identity;

- appearance;

- locked attributes;

- character reference;

- scene usage.

Character Module không trực tiếp thực hiện image generation.

# **17. Scene Module**

Trách nhiệm:

- scene creation;

- ordering;

- scene content;

- active characters;

- scene-specific state;

- selected output reference.

# **18. Generation Module**

Generation Module quản lý:

- generation request;

- job;

- attempt;

- candidate;

- output status;

- selection;

- regeneration;

- history.

Generation Module không chứa model-specific implementation.

# **19. Asset Module**

Asset Module quản lý:

- file metadata;

- image upload;

- generated images;

- thumbnail;

- asset ownership;

- asset reference.

# **20. AI Integration Module**

Đây là application-facing gateway tới AI layer.

Các module khác không nên trực tiếp gọi model/provider.

Ví dụ:

Scene Module

↓

Generation Module

↓

AI Integration Module

↓

AI Worker

# **21. Canonical State Boundary**

Một nguyên tắc kiến trúc quan trọng:

AI Output

≠

Canonical State

AI-generated output phải đi qua application workflow trước khi được sử dụng làm dữ liệu chính thức.

# **22. Canonical Update Flow**

AI Generates Proposal

↓

Application Receives Candidate

↓

Validation

↓

User/System Decision

↓

Application Command

↓

Canonical State Updated

Giải thích bằng lời:

Model không trực tiếp update Character hay Scene. Output đầu tiên được xem như candidate hoặc proposal. Chỉ khi workflow quyết định chấp nhận thì backend mới thực hiện canonical update.

# **23. Main Domain Relationships**

Ở mức khái niệm:

User

↓

Project

↓

Story

├── Characters

├── Scenes

└── Assets

Một Scene có thể chứa nhiều Character.

Một Character có thể xuất hiện trong nhiều Scene.

Đây là quan hệ many-to-many giữa Scene và Character.

# **24. Scene–Character State**

Scene không chỉ cần biết Character nào xuất hiện mà còn cần state của Character trong Scene đó.

Do đó tồn tại logical relation:

Scene

↓

SceneCharacter

↓

Character

SceneCharacter có thể chứa:

- outfit;

- emotion;

- action;

- scene appearance;

- temporary state.

# **25. Generation Domain Model**

Một generation logical structure:

Generation Request

↓

Generation Job

↓

Generation Attempt

↓

Candidate

↓

Asset

Một Job có thể có nhiều attempts.

Một attempt có thể sinh nhiều candidates.

# **26. Context Snapshot**

Generation Job phải tham chiếu tới Context Snapshot.

Generation Job

↓

Context Snapshot

↓

Story / Character / Scene Versions

Context Snapshot là immutable.

Nó cho biết chính xác AI đã nhìn thấy dữ liệu gì tại thời điểm generation.

# **27. Context Builder Boundary**

Context Builder không truy xuất toàn bộ Project một cách tùy tiện.

Nó nhận generation intent và xác định context cần thiết.

Ví dụ Scene 12:

Story Summary

Scene 12

Character A

Character B

Relevant Continuity

Style

Không nhất thiết đưa Character C nếu C không xuất hiện.

# **28. AI Model Abstraction**

Backend không gọi cụ thể:

StoryDiffusion.Generate()

hoặc:

OpenAI.Generate()

trong business module.

Thay vào đó sử dụng abstraction:

ITextGenerationService

IImageGenerationService

IConsistencyService

Implementation cụ thể nằm ở Infrastructure/AI Adapter.

# **29. Why AI Abstraction Matters**

Nó cho phép:

- StoryDiffusion hôm nay;

- research model mới ngày mai;

- external API nếu cần;

- local GPU worker;

- cloud inference.

Product workflow không thay đổi.

# **30. Asynchronous Boundary**

Những operation sau nên asynchronous:

- image generation;

- long text generation;

- large consistency validation;

- batch generation;

- expensive preprocessing.

Những operation đơn giản như:

- edit Character;

- create Scene;

- select Candidate;

được xử lý synchronous.

# **31. Synchronous Request Flow**

Ví dụ update Scene:

Frontend

↓

API

↓

Application Service

↓

Domain Validation

↓

Database

↓

Response

Giải thích bằng lời:

User sửa Scene, frontend gửi API request. Backend kiểm tra dữ liệu rồi lưu database và trả kết quả ngay. Không cần queue hoặc AI worker.

# **32. Asynchronous Generation Flow**

Frontend

↓

POST Generate

↓

Backend

↓

Create Generation Job

↓

Queue

↓

Worker

↓

AI Model

↓

Validation

↓

Persist Candidate

↓

Frontend receives status/result

Giải thích bằng lời:

Frontend không chờ model sinh ảnh ngay trong HTTP request. Backend chỉ tạo job rồi trả job ID. Worker xử lý phía sau. Khi hoàn thành, frontend nhận trạng thái mới thông qua polling, event hoặc realtime channel.

# **33. Queue Responsibility**

Queue dùng để:

- buffer generation jobs;

- tránh request timeout;

- giới hạn concurrency;

- retry;

- scale workers;

- ưu tiên workload.

Queue không phải database canonical.

# **34. Worker Responsibility**

Worker chịu trách nhiệm:

- nhận job;

- load input snapshot;

- gọi AI adapter;

- xử lý raw output;

- chạy validation cần thiết;

- ghi result;

- cập nhật job status.

Worker không được trực tiếp chỉnh Story Core.

# **35. Worker Statelessness**

Worker nên gần stateless nhất có thể.

Mỗi job phải có đủ identifier để worker truy xuất input.

Điều này giúp:

- restart worker;

- scale horizontal;

- retry job.

# **36. Asset Storage Architecture**

Generated image flow:

AI Worker

↓

Image Output

↓

Asset Storage

↓

Asset Record

↓

Candidate

Database không nên chứa raw image binary cho workflow chính.

# **37. Asset Immutability**

Generated asset đã tạo nên được xem là immutable.

Regeneration tạo asset mới.

Không overwrite file cũ.

# **38. Selected Output**

Scene không lưu image trực tiếp.

Scene giữ:

selectedCandidateId

Candidate tham chiếu:

assetId

Nhờ vậy đổi candidate không cần copy image.

# **39. Event Model**

Application có thể phát domain/application events.

Ví dụ:

CharacterUpdated

SceneUpdated

GenerationRequested

GenerationCompleted

CandidateSelected

Event giúp giảm coupling giữa module.

# **40. Character Update Event**

Ví dụ:

CharacterUpdated

↓

Dependency Analyzer

↓

Find affected Scenes

↓

Mark relevant outputs stale

Giải thích bằng lời:

Character Module không cần tự biết toàn bộ logic generation. Nó chỉ phát sự kiện CharacterUpdated. Thành phần dependency tracking có thể phản ứng và đánh dấu output liên quan dùng context cũ.

# **41. Dependency Tracking**

Hệ thống cần biết generated output phụ thuộc vào version nào của:

- Story;

- Scene;

- Character;

- Style;

- References.

Thông tin này được lấy từ snapshot/provenance metadata.

# **42. Stale Detection**

Ví dụ:

Image Candidate X sử dụng:

Character A version 4

Character A hiện tại:

version 5

System có thể suy ra:

Candidate X = potentially stale

Không cần xóa Candidate X.

# **43. Consistency Service**

Consistency Service chịu trách nhiệm đánh giá các vấn đề như:

- identity drift;

- character blending;

- reference mismatch;

- scene continuity.

Service này có thể sử dụng:

- deterministic rules;

- vision model;

- embedding similarity;

- research algorithm.

Architecture không khóa implementation cụ thể.

# **44. Validation Pipeline**

Generation result đi qua:

Technical Validation

↓

Schema / Contract Validation

↓

Semantic Validation

↓

Consistency Validation

↓

Policy Validation

Chi tiết được định nghĩa trong AI-03.

# **45. Authentication Boundary**

Identity/Access module chịu trách nhiệm:

- authentication;

- current user;

- project ownership;

- authorization.

Các business module không tự triển khai login logic riêng.

# **46. Authorization Rule**

Backend phải kiểm tra authorization tại server.

Không dựa vào việc frontend ẩn button.

# **47. API Boundary**

Frontend chỉ tương tác backend thông qua public application API.

Không được:

- truy database trực tiếp;

- gọi AI worker trực tiếp;

- gọi queue trực tiếp.

# **48. Internal Service Communication**

Trong modular monolith, module có thể giao tiếp qua:

- application interface;

- domain event;

- internal message.

Không nên truy cập table của module khác một cách tùy tiện.

# **49. Transaction Boundary**

Các canonical update nhỏ nên transaction atomic.

Ví dụ:

Update Character

\+

Create Character Version

\+

Publish update record

Nếu transaction thất bại thì không commit partial state.

# **50. AI Transaction Rule**

Không giữ database transaction mở trong suốt AI generation.

Ví dụ sai:

BEGIN TRANSACTION

→ Call AI for 90 seconds

→ Save

COMMIT

AI generation phải nằm ngoài transaction dài.

# **51. Caching**

Cache có thể dùng cho:

- Project summaries;

- Character context;

- expensive derived data;

- read-heavy metadata.

Canonical truth vẫn nằm trong persistence layer.

Cache invalidation phải gắn với version/state change.

# **52. Observability**

Architecture phải hỗ trợ ba nhóm observability.

### **Logs**

Theo dõi lỗi và operation.

### **Metrics**

Ví dụ:

- API latency;

- generation latency;

- job failure rate;

- queue depth.

### **Traces**

Theo dõi một operation xuyên qua:

API → Job → Worker → AI Provider

# **53. Correlation**

Mỗi generation cần có correlation identifiers:

- requestId;

- jobId;

- attemptId.

Developer phải có khả năng tìm toàn bộ log liên quan tới một generation.

# **54. Error Boundaries**

Error được chia ít nhất:

- application error;

- validation error;

- infrastructure error;

- AI provider error;

- generation quality failure.

Không trả raw exception trực tiếp cho frontend.

# **55. Retry Boundary**

Retry chỉ áp dụng cho operation an toàn hoặc idempotent.

Ví dụ:

- provider timeout;

- temporary network error.

Không tự retry canonical mutation nếu có nguy cơ tạo duplicate mà không có idempotency protection.

# **56. Security Principles**

Hệ thống cần đảm bảo:

- ownership validation;

- input validation;

- secure asset access;

- secret management;

- rate limiting khi cần;

- không expose provider API keys ra frontend.

# **57. AI Credential Security**

Frontend không được chứa:

- model API key;

- cloud inference token;

- private storage credential.

Worker/backend mới giữ secret.

# **58. Asset Access**

Asset URL có thể là:

- authenticated endpoint;

- signed URL;

- controlled public asset;

tùy deployment.

Canonical private project asset không mặc định public.

# **59. Data Ownership**

Mọi domain object phải xác định được ownership thông qua Project/User.

Ví dụ:

Candidate

→ Scene

→ Story

→ Project

→ User

# **60. Scalability**

Architecture phải cho phép scale riêng:

### **Web/API Layer**

Theo số user/request.

### **AI Worker Layer**

Theo generation load/GPU.

### **Storage**

Theo số asset.

### **Queue**

Theo pending jobs.

# **61. AI Worker Scaling**

Worker có thể scale horizontal:

Queue

├── Worker 1

├── Worker 2

├── Worker 3

└── Worker N

Worker không nên phụ thuộc vào local session state.

# **62. GPU-Aware Processing**

Nếu model chạy trên GPU, worker architecture có thể chia:

- CPU preprocessing worker;

- GPU inference worker;

- validation worker.

MVP không bắt buộc tách thành ba deployment độc lập.

# **63. Research Integration Boundary**

Research model phải được tích hợp thông qua AI adapter.

Ví dụ:

IImageGenerationService

↓

StoryDiffusionAdapter

Sau này:

IImageGenerationService

↓

ProposedMethodAdapter

Application không cần thay đổi Scene workflow.

# **64. Experiment Compatibility**

Generation metadata nên cho phép lưu:

- model identifier;

- experiment configuration;

- method version.

Điều này cho phép cùng application hỗ trợ:

- baseline;

- proposed method;

- comparison.

# **65. Locked MVP Technology Mapping**

Technology stack cho implementation MVP được khóa như sau:

### **Frontend**

Vue 3 + Vite + TypeScript.

Vue Router được dùng cho navigation. State management có thể sử dụng Pinia khi shared workspace state thực sự cần thiết.

### **Backend**

ASP.NET Core Web API.

### **Database**

PostgreSQL. pgvector được sử dụng khi Context/Memory retrieval cần vector search.

### **Queue**

RabbitMQ.

RabbitMQ là runtime queue chính giữa ASP.NET Core Application Backend và Python AI Worker. Queue implementation không được rải trực tiếp vào business modules; application phải đi qua queue abstraction/infrastructure boundary.

### **Asset Storage**

Application sử dụng IAssetStorage abstraction.

Development:

LocalFileAssetStorage

Demo / production-like:

S3AssetStorage với S3-compatible provider, ví dụ Cloudflare R2.

Business/domain code không phụ thuộc trực tiếp provider cụ thể.

### **Realtime**

SignalR là optional realtime UX layer cho Job progress/event notification. REST Job API vẫn là source of truth.

### **AI Worker**

Python.

### **AI Runtime**

PyTorch / Diffusers / custom research code.

### **Locked Runtime Summary**

Vue 3 + Vite + TypeScript

↓

ASP.NET Core

↓

PostgreSQL + RabbitMQ + Asset Storage

↓

Python AI Worker

↓

StoryDiffusion / Proposed Method (SAMIM) / External AI Adapter

# **66. Why ASP.NET + Python Boundary**

Backend product logic phù hợp với ASP.NET Core.

Research AI ecosystem chủ yếu dùng Python.

Do đó có thể tách:

ASP.NET Application Backend

↓

Queue/API

↓

Python AI Worker

Giải thích bằng lời:

ASP.NET chịu trách nhiệm nghiệp vụ, user, story, database và job orchestration. Python chịu trách nhiệm model và inference. Hai phần giao tiếp qua contract rõ ràng thay vì cố nhét AI code Python vào backend .NET.

# **67. Development Environment**

Local development có thể chạy:

- frontend;

- ASP.NET backend;

- PostgreSQL;

- Redis/queue;

- Python worker;

- local/file object storage.

Các component có thể được containerized.

# **68. Deployment Independence**

ARCH-01 không bắt buộc:

- AWS;

- Azure;

- GCP.

ARCH-02 sẽ định nghĩa deployment topology chi tiết hơn.

# **69. Architecture Constraints**

## **ARC-01**

Frontend SHALL NOT trực tiếp truy database.

## **ARC-02**

Frontend SHALL NOT trực tiếp gọi private AI inference service.

## **ARC-03**

AI Worker SHALL NOT trực tiếp update canonical Story/Character data.

## **ARC-04**

Long-running AI task SHALL NOT giữ synchronous HTTP request mở nếu vượt runtime phù hợp.

## **ARC-05**

Generated assets SHALL được lưu ngoài relational domain table dưới dạng binary reference.

## **ARC-06**

Generation SHALL có provenance.

## **ARC-07**

Context Snapshot SHALL immutable sau khi generation bắt đầu.

## **ARC-08**

Model-specific implementation SHALL nằm sau AI abstraction.

# **70. Functional Architecture Requirements**

## **ARCH-FR-01**

System SHALL cung cấp application API cho frontend.

## **ARCH-FR-02**

System SHALL có background execution mechanism cho long-running AI jobs.

## **ARCH-FR-03**

System SHALL lưu canonical domain state trong persistence layer.

## **ARCH-FR-04**

System SHALL tách generated assets khỏi structured domain data.

## **ARCH-FR-05**

System SHALL hỗ trợ generation status tracking.

## **ARCH-FR-06**

System SHALL hỗ trợ multiple generation attempts.

## **ARCH-FR-07**

System SHALL hỗ trợ dependency/version tracking.

## **ARCH-FR-08**

System SHALL hỗ trợ AI implementation replacement thông qua adapter boundary.

## **ARCH-FR-09**

System SHALL hỗ trợ character-aware scene context construction.

## **ARCH-FR-10**

System SHALL giữ selected output độc lập với generation history.

# **71. Non-Functional Architecture Requirements**

## **ARCH-NFR-01 — Maintainability**

Module boundary phải rõ ràng để developer có thể thay đổi một subsystem mà hạn chế ảnh hưởng subsystem khác.

## **ARCH-NFR-02 — Reliability**

AI failure không được corrupt domain state.

## **ARCH-NFR-03 — Scalability**

AI worker phải có khả năng scale độc lập với web application.

## **ARCH-NFR-04 — Observability**

Generation operation phải có log và correlation identifiers.

## **ARCH-NFR-05 — Security**

Secret và private infrastructure credential không được expose ra client.

## **ARCH-NFR-06 — Extensibility**

Model và provider mới có thể được thêm thông qua adapter.

# **72. Primary Runtime Scenario**

Scene image generation chạy như sau:

User

↓

Frontend

↓

Application API

↓

Generation Module

↓

Context Builder

↓

Create Snapshot

↓

Create Job

↓

Queue

↓

AI Worker

↓

Image Model

↓

Validation

↓

Asset Storage

↓

Candidate Record

↓

Frontend

Giải thích bằng lời:

User yêu cầu sinh ảnh. Backend không gửi Scene trực tiếp tới model. Trước tiên hệ thống xây context, lưu snapshot, tạo Job rồi worker mới thực hiện generation. Kết quả được validation, lưu thành asset và candidate. Frontend chỉ nhận candidate sau khi pipeline đã xử lý.

# **73. Canonical Character Update Scenario**

User edits Character

↓

Character Module

↓

Validate

↓

Save New Version

↓

CharacterUpdated Event

↓

Dependency Analysis

↓

Affected Candidates Marked Stale

Giải thích bằng lời:

Khi user chỉnh Character, hệ thống chỉ thay canonical Character và tạo version mới. Output cũ không bị xóa. Dependency analysis tìm những generation đã sử dụng version trước để đánh dấu chúng có thể outdated.

# **74. Architecture Decisions**

## **ADR-01**

Use modular monolith for core product backend in MVP.

## **ADR-02**

Separate AI execution into worker boundary.

## **ADR-03**

Use asynchronous jobs for expensive AI operations.

## **ADR-04**

Store binary assets outside relational domain data.

## **ADR-05**

Use immutable context snapshots for generation.

## **ADR-06**

Use provider/model adapters for AI integration.

## **ADR-07**

Canonical state may only be updated through Application Layer.

# **75. Dependencies**

ARCH-01 phụ thuộc vào:

- PROD-01 product requirements;

- PROD-02 workflows;

- UX interaction requirements;

- AI-01 orchestration;

- AI-02 context/memory;

- AI-03 generation/validation.

ARCH-01 là parent architecture cho:

- Data design;

- API design;

- deployment architecture.

# **76. Out of Scope**

ARCH-01 không đặc tả chi tiết:

- physical database schema;

- endpoint URL;

- queue technology;

- container configuration;

- Kubernetes;

- cloud networking;

- CI/CD;

- GPU instance type;

- exact AI model implementation.

Các nội dung đó thuộc tài liệu tiếp theo hoặc nhóm chuyên môn khác.

# **77. Architecture Principle**

Nguyên tắc trung tâm:

> **The application owns the story; AI services perform bounded creative work around it.**

Nói đơn giản:

**OWNIVERSE quản lý câu chuyện.  
AI chỉ nhận nhiệm vụ, tạo kết quả và trả kết quả về.**

AI không được trở thành nơi giữ trạng thái sản phẩm.

# **78. Architecture Summary**

Toàn bộ hệ thống có thể nhớ bằng bốn phần:

**Frontend  
**→ người dùng tương tác.

**Application Backend  
**→ quản lý story và business rules.

**AI Worker Layer  
**→ thực hiện generation và validation.

**Database + Asset Storage  
**→ lưu trạng thái và kết quả.

Luồng tổng quát là:

> **User → Application → Job → AI → Validation → Candidate → User Decision → Canonical State**

Đây là backbone kỹ thuật mà các tài liệu Data, API, AI và Deployment phía sau sẽ triển khai chi tiết.
