# **DEPLOY-01 — Local Development & Environment Configuration**

**Document ID:** DEPLOY-01  
**Document Type:** Local Development & Environment Configuration Specification  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Primary Roles:** Developer / DevOps Engineer / AI Engineer / Thesis Author  
**Status:** Draft v1 — Updated Implementation Baseline  
**Parent Documents:** ARCH-01, ARCH-02  
**Related Documents:** DEV-01, IMPLEMENT-01, IMPLEMENT-02, DATA-01, DATA-02, API-01, API-02, AI-01, AI-02, AI-03, DEPLOY-02, DEPLOY-03, QA-01, QA-03

# **1. Purpose**

Tài liệu này định nghĩa cách thiết lập và vận hành môi trường phát triển cục bộ cho OWNIVERSE theo một cấu hình có thể tái lập, tách biệt và đủ gần với runtime thực tế để hỗ trợ phát triển, kiểm thử và tích hợp AI.

Mục tiêu chính của DEPLOY-01 là đảm bảo một developer hoặc AI coding assistant có thể clone repository, cấu hình các dependency cần thiết và khởi động hệ thống mà không cần hiểu toàn bộ deployment production-like.

DEPLOY-01 tập trung vào:

- local development topology;

- environment configuration;

- dependency bootstrap;

- database migration;

- asset storage development mode;

- queue configuration;

- AI Worker execution modes;

- health/readiness checks;

- startup/shutdown order;

- seed data;

- smoke verification;

- troubleshooting;

- onboarding flow.

Tài liệu này không thay thế DEPLOY-02 hoặc DEPLOY-03.

# **2. Deployment Principle**

Môi trường development phải tuân theo nguyên tắc:

> **Development should be reproducible, isolated, provider-replaceable and safe for iteration.**

Local setup không được buộc developer phải có GPU mạnh hoặc account cloud cụ thể mới có thể phát triển application core.

Application phải chạy được ngay cả khi real AI model chưa sẵn sàng.

# **3. Locked Implementation Stack**

Technology stack hiện tại của OWNIVERSE được khóa cho implementation như sau.

## **3.1 Frontend**

Vue 3

\+

Vite

\+

TypeScript

Frontend chịu trách nhiệm presentation, interaction và gọi public application API.

Frontend không trực tiếp truy database, queue, object storage hoặc AI provider.

## **3.2 Backend**

ASP.NET Core Web API

Backend chịu trách nhiệm:

- authentication/authorization;

- canonical state;

- business rules;

- revision/concurrency;

- context snapshot creation;

- job creation;

- API contract;

- asset metadata;

- orchestration coordination.

## **3.3 Relational Database**

PostgreSQL

PostgreSQL là canonical structured-data store của MVP.

## **3.4 Queue**

RabbitMQ

RabbitMQ được sử dụng cho asynchronous work dispatch giữa ASP.NET Core backend và Python AI Worker.

## **3.5 AI Runtime**

Python AI Worker

Python Worker thực hiện model inference, AI adapter execution và validation pipeline theo Job contract.

## **3.6 Asset Storage**

Asset storage nằm sau abstraction:

IAssetStorage

Development implementation:

LocalFileAssetStorage

Demo/production-like implementation:

S3AssetStorage

Provider ưu tiên hiện tại cho demo/production-like là Cloudflare R2 hoặc dịch vụ tương thích S3, nhưng provider không được hard-code vào Application hoặc Domain Layer.

## **3.7 Realtime**

SignalR là optional enhancement.

Hệ thống phải hoạt động đầy đủ bằng REST Job API và polling ngay cả khi realtime layer không chạy.

# **4. Local Runtime Topology**

Development topology tiêu chuẩn:

Browser

↓

Vue 3 + Vite Frontend

↓

ASP.NET Core API

├── PostgreSQL

├── RabbitMQ

└── LocalFileAssetStorage

↓

Shared Asset Directory

RabbitMQ

↓

Python AI Worker

↓

AI Adapter

├── Mock

├── StoryDiffusion

├── SAMIM / Proposed Method

└── External API Adapter

Backend và Worker không chia sẻ business state bằng in-memory state.

Job, Snapshot, Candidate và canonical records phải có durable representation phù hợp.

# **5. Repository Assumption**

Logical repository structure:

/

├── frontend/

├── backend/

├── ai-worker/

├── tests/

├── deployment/

├── docs/

├── scripts/

├── .github/

├── README.md

└── .gitignore

Deployment-related development files nên nằm trong:

deployment/

├── compose/

├── env/

├── scripts/

└── local/

Exact physical structure có thể điều chỉnh trong Sprint 0 nhưng không được phá các boundary trong DEV-01.

# **6. Development Profiles**

OWNIVERSE sử dụng ba development profile chính.

## **6.1 MOCK_AI**

Mục đích:

- frontend development;

- backend development;

- Job lifecycle testing;

- Candidate workflow testing;

- integration testing không cần GPU.

Topology:

Frontend

↓

API

↓

RabbitMQ

↓

Python Worker

↓

MockImageGenerationAdapter

Mock provider nên hỗ trợ ít nhất:

- sample image/result;

- configurable artificial delay;

- simulated technical failure;

- simulated validation warning.

Suggested default artificial delay:

~3 seconds

Delay này chỉ phục vụ kiểm thử async UX, không phải product requirement.

## **6.2 FULL_LOCAL**

Mục đích:

- chạy application và AI model trên cùng development machine;

- test real generation khi hardware đủ khả năng.

Topology:

Frontend

API

PostgreSQL

RabbitMQ

Local Storage

Python Worker

Local AI Model

FULL_LOCAL không phải profile bắt buộc cho mọi developer.

Nếu máy không đủ GPU, developer được phép dùng MOCK_AI hoặc REMOTE_AI.

## **6.3 REMOTE_AI**

Mục đích:

- development với GPU remote;

- Colab/research runtime;

- external GPU machine;

- temporary thesis demo worker.

Topology:

Local Frontend/API

↓

RabbitMQ

↓

Remote Python Worker

↓

Remote GPU / Model

Hoặc nếu remote worker không thể kết nối trực tiếp vào queue, một controlled adapter boundary có thể được dùng theo DEPLOY-02/03.

Remote AI không được làm thay đổi Application contract.

# **7. Environment Configuration Principle**

Configuration phải externalized khỏi source code.

Không hard-code:

- database password;

- RabbitMQ credential;

- S3/R2 secret;

- external AI API key;

- model endpoint token;

- production-like URLs.

Configuration được cung cấp qua:

- environment variables;

- local .env files không commit secret;

- development secret store nếu sử dụng;

- CI/CD secret store trong tương lai.

# **8. Core Environment Variables**

Naming có thể được map sang ASP.NET/Python convention cụ thể, nhưng logical variables gồm:

APP_ENV

APP_BASE_URL

FRONTEND_BASE_URL

DATABASE_URL

QUEUE_URL

QUEUE_NAME

ASSET_STORAGE_PROVIDER

ASSET_STORAGE_PATH

ASSET_STORAGE_URL

AI_PROVIDER

AI_WORKER_MODE

MODEL_ID

MODEL_PATH

MODEL_ENDPOINT

MAX_RETRIES

WORKER_CONCURRENCY

JOB_HEARTBEAT_INTERVAL

JOB_LEASE_TIMEOUT

CONTEXT_SCHEMA_VERSION

PROMPT_TEMPLATE_VERSION

ADAPTER_VERSION

METHOD_VERSION

External-provider mode có thể cần thêm secret variables riêng.

# **9. Asset Storage Configuration**

## **9.1 Development**

Default:

ASSET_STORAGE_PROVIDER=local

Implementation:

LocalFileAssetStorage

Binary asset được lưu vào development volume/directory.

Database chỉ lưu metadata và locator/key cần thiết.

Local storage directory phải:

- nằm ngoài compiled application output;

- có thể mount qua Docker volume;

- có permission rõ ràng;

- không được commit generated assets vào Git.

## **9.2 Demo / Production-like**

Configured implementation:

S3AssetStorage

Có thể kết nối Cloudflare R2 hoặc S3-compatible provider thông qua configuration.

Application code không được branch business behavior theo tên Cloudflare/AWS.

# **10. Database Setup**

PostgreSQL development instance có thể chạy bằng Docker Compose.

Developer không bắt buộc cài PostgreSQL trực tiếp lên host machine.

Database initialization gồm:

Start PostgreSQL

↓

Wait until ready

↓

Apply migrations

↓

Optional seed

↓

Start API

Application startup không nên âm thầm tạo production schema không kiểm soát.

Trong development có thể hỗ trợ convenience command để apply migration, nhưng migration vẫn phải version-controlled.

# **11. Migration Rules**

Mọi schema change phải có migration.

Không được:

Edit database manually

→ forget migration

Mỗi migration phải:

- có tên có ý nghĩa;

- chạy được từ clean database;

- chạy được trên database version trước đó trong phạm vi supported development path;

- được review cùng feature liên quan.

Sprint 0 phải xác nhận:

> clean database → apply all migrations → API starts successfully.

# **12. Seed Data**

Seed data chỉ dùng để tăng tốc development và demo.

Seed có thể bao gồm:

- development user;

- example project;

- example story;

- two example characters;

- sample scenes;

- sample reference metadata.

Không seed secret hoặc production user data.

Seed command phải explicit.

Recommended distinction:

seed:dev

seed:test

seed:demo

Exact command names được quyết định trong implementation.

# **13. RabbitMQ Setup**

RabbitMQ là queue implementation mặc định.

Development RabbitMQ nên chạy containerized.

Required behavior:

- backend publish Job message;

- worker consume Job message;

- worker acknowledge sau khi claim/processing semantics hợp lệ;

- transient worker crash không làm Job biến mất khỏi durable system state;

- Job database record vẫn là source of truth.

RabbitMQ message không phải canonical Job record.

# **14. Queue Development Configuration**

Logical configuration:

QUEUE_URL=...

QUEUE_NAME=owniverse-generation

Có thể có queue riêng sau này cho text/image/validation, nhưng MVP không cần chia queue nếu chưa có nhu cầu thực tế.

Không premature optimize topology.

# **15. Python Worker Setup**

Python Worker là process độc lập với ASP.NET Core API.

Development setup phải hỗ trợ:

python worker

hoặc containerized worker.

Worker startup phải:

1.  load configuration;

2.  initialize adapter registry;

3.  test required infrastructure connectivity;

4.  register readiness state;

5.  begin Job consumption.

# **16. Worker Modes**

Worker phải có khả năng chạy ít nhất các mode sau.

## **MOCK**

AI_PROVIDER=mock

Không cần GPU.

## **BASELINE**

AI_PROVIDER=storydiffusion

Chạy StoryDiffusionAdapter khi environment hỗ trợ.

## **PROPOSED**

AI_PROVIDER=samim

Chỉ dùng sau khi research method đủ ổn định.

## **EXTERNAL**

AI_PROVIDER=external

Adapter kết nối image API/provider bên ngoài.

Không mode nào được thay đổi Application workflow.

# **17. Generation Context Configuration**

Application-to-AI contract không phải raw prompt string.

Contract chính:

GenerationContextV1

Flow:

Canonical Domain Data

↓

GenerationContextV1

↓

Immutable Context Snapshot

↓

AI Adapter

↓

Adapter-specific Prompt / Conditioning Builder

↓

Model Input

Local environment phải sử dụng cùng contract này cho Mock, StoryDiffusion và SAMIM.

Mock mode không được bypass context construction nếu mục tiêu là integration test.

# **18. Prompt/Model Input Provenance**

Development logs/records cần hỗ trợ truy vết:

contextSchemaVersion

promptTemplateVersion or conditionBuilderVersion

adapterVersion

modelId

methodVersion

seed

Raw rendered prompt hoặc model-specific payload có thể được lưu như private provenance/debug artifact nếu phù hợp.

Không expose technical prompt mặc định ra public frontend API.

# **19. Local AI Hardware Independence**

OWNIVERSE development không giả định mọi developer đều có GPU đủ mạnh.

Nếu local machine không chạy được baseline/proposed method, developer có thể dùng:

MOCK_AI

hoặc:

REMOTE_AI

Điều này là requirement kiến trúc, không phải workaround tạm thời.

# **20. Colab / Remote GPU Boundary**

Google Colab hoặc GPU notebook environment có thể dùng cho:

- research experiments;

- baseline reproduction;

- development integration;

- temporary thesis demo support.

Colab không được coi là long-term production infrastructure.

Application không được phụ thuộc vào notebook lifecycle để giữ canonical state.

Nếu remote runtime chết:

- Project/Story/Character/Scene vẫn an toàn;

- Job có thể fail/retry theo contract;

- Candidate đã persist không được mất.

# **21. Frontend Local Setup**

Frontend development workflow:

install dependencies

↓

configure API base URL

↓

start Vite dev server

Frontend environment chỉ chứa public/non-secret configuration.

Không đặt database credentials, queue credentials hoặc AI provider secret trong frontend build.

# **22. Backend Local Setup**

Backend workflow:

restore dependencies

↓

load development configuration

↓

verify PostgreSQL

↓

verify RabbitMQ

↓

verify Asset Storage

↓

apply/confirm migrations

↓

start ASP.NET Core API

API nên fail fast nếu critical dependency bắt buộc không tồn tại.

Optional dependency như SignalR enhancement hoặc real AI provider không được làm API core không khởi động nếu profile hiện tại không yêu cầu.

# **23. Docker Compose Development Mode**

Docker Compose nên được sử dụng ít nhất cho infrastructure dependencies.

Recommended infrastructure services:

postgres

rabbitmq

Local asset storage không cần object-storage server riêng; có thể mount shared filesystem volume qua LocalFileAssetStorage.

Application containers có thể được thêm theo DEPLOY-02.

Sprint 0 có thể bắt đầu bằng:

Host Frontend

Host API

Host Worker

Docker PostgreSQL

Docker RabbitMQ

sau đó chuyển dần sang full container mode.

# **24. Startup Order**

Recommended local startup sequence:

1\. PostgreSQL

2\. RabbitMQ

3\. Local asset directory/volume

4\. Database migration

5\. ASP.NET Core API

6\. Python Worker

7\. Vue Frontend

Trong MOCK_AI, worker vẫn nên chạy để kiểm thử queue/job pipeline thật.

# **25. Shutdown Order**

Development shutdown có thể đơn giản hơn production, nhưng Worker phải hỗ trợ graceful behavior.

Worker shutdown sequence:

Stop claiming new jobs

↓

Finish or safely checkpoint current work where possible

↓

Release lease/ownership when required

↓

Flush logs/status

↓

Stop runtime

API shutdown không được xóa Job state.

# **26. Health Model**

Health được tách thành:

Liveness

Readiness

Liveness trả lời:

> process còn sống không?

Readiness trả lời:

> process đã đủ điều kiện nhận traffic/work chưa?

# **27. API Health Checks**

API health nên kiểm tra:

- application process;

- PostgreSQL connectivity;

- RabbitMQ connectivity;

- Asset Storage availability khi required.

Suggested endpoints:

/health/live

/health/ready

Exact route có thể được chốt trong implementation.

# **28. Worker Health States**

Worker có thể biểu diễn các trạng thái:

STARTING

READY

BUSY

DEGRADED

UNHEALTHY

BUSY không đồng nghĩa UNHEALTHY.

Worker đang inference vẫn có thể healthy.

# **29. Frontend Health Behavior**

Frontend không cần tự kiểm tra trực tiếp Database/Queue/Worker.

Frontend chỉ tương tác với backend contract.

Nếu AI runtime unavailable, backend trả trạng thái/error phù hợp thay vì frontend cố đoán provider state.

# **30. Logging in Development**

Logs nên đủ để truy vết một generation flow.

Khi applicable, log:

requestId

jobId

attemptId

candidateId

workerId

adapterId

modelId

Không log:

- secret;

- access token;

- password;

- full sensitive story content nếu không cần;

- raw external API credentials.

# **31. Development Observability**

MVP local observability có thể dựa vào:

- structured console logs;

- RabbitMQ management UI;

- database inspection;

- Job status endpoint;

- worker status logs.

Không cần triển khai full observability platform trong Sprint 0.

# **32. Local Asset Inspection**

Development có thể cho phép developer inspect generated assets trực tiếp trong local storage directory.

Tuy nhiên application vẫn phải truy asset qua IAssetStorage/asset service abstraction.

Không viết code business phụ thuộc filesystem path thật.

# **33. Configuration Profiles**

Suggested application profiles:

Development.MockAI

Development.LocalAI

Development.RemoteAI

Test

Demo

ProductionLike

Exact framework-specific naming có thể khác.

Điều quan trọng là profile phải thay configuration, không thay business logic.

# **34. Secret Handling**

Repository chỉ được chứa template:

.env.example

appsettings.Development.example.json

Secret thực tế không được commit.

.gitignore phải cover local secret files.

Nếu accidental secret commit xảy ra, secret phải được rotate chứ không chỉ xóa khỏi latest commit.

# **35. Local Reset Strategy**

Development phải có cách reset environment có kiểm soát.

Có thể gồm:

stop services

↓

remove development database volume

↓

remove development asset volume if explicitly requested

↓

restart services

↓

apply migrations

↓

seed development data

Không tạo command reset production-like environment theo default.

# **36. Database Reset Safety**

Reset script phải yêu cầu environment rõ ràng.

Ví dụ:

APP_ENV=Development

Không chạy destructive reset nếu environment không phải development/test.

# **37. Local Storage Reset Safety**

Asset cleanup phải tách khỏi database cleanup nếu có thể.

Lý do:

- developer có thể cần giữ asset để debug;

- database reset không nhất thiết cần xóa large generated files;

- tránh accidental data loss.

# **38. Smoke Verification**

Sau setup, developer phải chạy được flow tối thiểu:

Create/Open Project

↓

Create Character

↓

Upload Reference

↓

Create Scene

↓

Assign Character

↓

Generate

↓

Job completes

↓

Candidate appears

↓

Select Candidate

↓

Reload

Trong Sprint 0–4, flow này có thể được rút gọn theo feature hiện có.

Từ Sprint 5 trở đi, full Mock generation smoke flow phải chạy.

# **39. Infrastructure Smoke Checks**

Before application smoke test:

\[ \] PostgreSQL accepts connection

\[ \] RabbitMQ accepts connection

\[ \] Local asset directory writable

\[ \] API health ready

\[ \] Worker ready

\[ \] Frontend can call API

# **40. Mock AI Verification**

MOCK_AI smoke test:

Generate request

→ Job queued

→ Worker receives

→ artificial delay

→ sample asset created

→ Candidate persisted

→ Job completed

A second test should simulate:

Generate request

→ Worker failure

→ Job failed/retry behavior visible

# **41. Real AI Verification**

Real model verification không phải Sprint 0 requirement.

Khi baseline integration bắt đầu:

Context Snapshot

→ StoryDiffusionAdapter

→ Model execution

→ Asset

→ Candidate

→ Validation

must use the same Job pipeline as Mock mode.

# **42. Development Error Categories**

Common environment error categories:

CONFIGURATION_ERROR

DATABASE_UNAVAILABLE

QUEUE_UNAVAILABLE

STORAGE_UNAVAILABLE

WORKER_UNAVAILABLE

MODEL_UNAVAILABLE

MIGRATION_ERROR

VERSION_MISMATCH

Exact public error keys follow API specification.

# **43. Troubleshooting — API Cannot Start**

Check in order:

1.  configuration loaded;

2.  PostgreSQL running;

3.  connection string valid;

4.  migrations valid;

5.  RabbitMQ reachable;

6.  asset directory permission;

7.  port conflict.

Do not immediately change application code when infrastructure is the cause.

# **44. Troubleshooting — Job Stuck QUEUED**

Check:

1.  Worker running;

2.  Worker READY;

3.  Queue name matches;

4.  RabbitMQ connectivity;

5.  Job message published;

6.  worker claim logs;

7.  Job lease/state consistency.

# **45. Troubleshooting — Worker Fails**

Check:

- Python environment;

- adapter configuration;

- model files;

- GPU/CUDA compatibility when real AI;

- memory availability;

- remote endpoint availability;

- storage write access.

If real AI fails, switch to MOCK_AI to determine whether the issue is model-specific or orchestration-specific.

# **46. Troubleshooting — Asset Missing**

Check:

Candidate metadata

↓

Asset record

↓

Storage key/path

↓

IAssetStorage implementation

↓

Physical file/object

Không sửa Candidate URL trực tiếp trong database để workaround.

# **47. Troubleshooting — Frontend Cannot Load**

Check:

- Vite server;

- API base URL;

- CORS configuration;

- backend health;

- auth state;

- API response/JSend handling.

Frontend không kết nối RabbitMQ trực tiếp.

# **48. Version Compatibility**

Development environment cần nhận diện versions quan trọng:

frontendVersion

apiVersion

workerVersion

contextSchemaVersion

adapterVersion

methodVersion

modelId

validationProfileVersion

Không nhất thiết expose toàn bộ cho user, nhưng cần cho debugging và thesis reproducibility.

# **49. Migration Compatibility**

API version mới không được giả định database schema đã được migrate nếu startup process chưa đảm bảo điều đó.

Recommended local flow:

migrate

then start API

DEPLOY-02 sẽ định nghĩa container orchestration cụ thể hơn.

# **50. Development Dependency Policy**

Infrastructure dependencies phải được pin/versioned ở mức hợp lý.

Không dùng uncontrolled latest image cho critical dependency trong reproducible thesis environment.

Recommended:

postgres:\<pinned-major/minor\>

rabbitmq:\<pinned-major/minor\>

Exact versions được khóa khi repository bootstrap thực hiện.

# **51. AI Dependency Policy**

Research/model dependencies cần environment riêng hoặc dependency lock phù hợp.

Không để experimental AI library upgrade tự động phá backend/frontend development environment.

Python Worker dependency boundaries nên tách:

core worker dependencies

baseline model dependencies

research model dependencies

nếu practical.

# **52. Local Development Without Real AI**

Developer mới phải có thể setup hệ thống mà không tải model rất lớn.

Required onboarding path:

Clone

↓

Start PostgreSQL + RabbitMQ

↓

Run migrations

↓

Start API

↓

Start Mock Worker

↓

Start Frontend

↓

Run smoke test

Đây là default onboarding path.

# **53. Local Development With Real AI**

Real AI onboarding là optional second path:

Complete normal onboarding

↓

Prepare AI environment

↓

Configure baseline adapter

↓

Download/prepare model assets

↓

Start Worker in baseline mode

↓

Run real generation smoke test

# **54. AI Coding Assistant Environment Rule**

Khi AI coding assistant chỉnh environment/deployment code, nó phải được cung cấp tối thiểu:

DEV-01

DEPLOY-01

Current Sprint section

Relevant ARCH section

Existing compose/config files

AI không được:

- đổi queue technology;

- đổi storage provider architecture;

- thêm cloud dependency;

- đổi environment variable semantics;

- bỏ Mock mode;

- hard-code provider credentials

mà không có quyết định kiến trúc mới.

# **55. CI Compatibility**

DEPLOY-01 không đặc tả full CI pipeline, nhưng local setup phải cho phép CI chạy:

- backend build;

- frontend build;

- migrations/integration DB;

- Mock Worker tests;

- integration tests.

CI không yêu cầu GPU cho mọi commit.

# **56. Test Environment**

Automated integration tests nên có isolated PostgreSQL/RabbitMQ instance hoặc equivalent disposable test infrastructure.

Không dùng developer's personal local database cho CI tests.

Asset storage test có thể dùng temporary filesystem implementation.

# **57. Environment Data Isolation**

Development, Test và Demo data phải tách nhau.

Không dùng chung database schema/data directory nếu có nguy cơ cross-environment contamination.

Recommended logical separation:

owniverse_dev

owniverse_test

owniverse_demo

# **58. Demo Environment Boundary**

Thesis Demo không được xem là local development.

Demo environment có thể sử dụng:

Web-hosted Frontend

ASP.NET Core API

PostgreSQL

RabbitMQ

S3-compatible Asset Storage

Remote GPU Worker

Chi tiết thuộc DEPLOY-03.

# **59. Production-like Boundary**

Production-like profile nhằm kiểm chứng:

- network separation;

- external storage;

- durable queue;

- deployment versions;

- monitoring;

- backup/recovery.

Không cần đạt enterprise-scale production architecture cho luận văn.

# **60. Security Baseline**

Development convenience không được phá security architecture.

Các rule vẫn áp dụng:

- backend enforces authorization;

- frontend secret-free;

- asset ownership checked server-side;

- uploaded file type/size validated;

- queue/storage credentials private;

- AI provider credentials private.

# **61. CORS**

Development có thể enable CORS cho local frontend origin cụ thể.

Không dùng permissive wildcard configuration làm production default.

# **62. HTTPS**

Local development có thể sử dụng framework development certificate hoặc HTTP trong isolated local context nếu cần.

Demo/production-like phải follow DEPLOY-03 HTTPS/reverse proxy rules.

# **63. Port Configuration**

Ports nên configurable.

Không viết frontend/backend logic phụ thuộc một port cố định ngoài environment config.

README phải nêu default development ports sau khi Sprint 0 khóa cấu hình.

# **64. README Requirements**

Repository root README phải có phần Quick Start tối thiểu:

Prerequisites

Environment setup

Start infrastructure

Run migrations

Start API

Start Worker

Start Frontend

Run smoke test

Troubleshooting link

README không thay thế DEPLOY-01; nó là onboarding summary.

# **65. Developer Onboarding Checklist**

Developer mới hoàn thành onboarding khi:

\[ \] Repository cloned

\[ \] Environment template copied

\[ \] PostgreSQL running

\[ \] RabbitMQ running

\[ \] Migrations applied

\[ \] API ready

\[ \] Mock Worker ready

\[ \] Frontend running

\[ \] Asset directory writable

\[ \] Smoke flow completed

# **66. Sprint 0 Deployment Deliverables**

Sprint 0 phải tạo ít nhất:

\[ \] local environment template

\[ \] PostgreSQL development service

\[ \] RabbitMQ development service

\[ \] LocalFileAssetStorage setup

\[ \] ASP.NET Core health endpoint

\[ \] Python Worker startup

\[ \] Mock worker message receive

\[ \] migration command/process

\[ \] frontend API configuration

\[ \] startup documentation

# **67. Sprint 0 Exit Criteria**

Sprint 0 deployment foundation được xem là hoàn thành khi:

Clean Machine / Clean Environment

↓

Configuration

↓

PostgreSQL + RabbitMQ Start

↓

Migrations Apply

↓

API Ready

↓

Mock Worker Ready

↓

Frontend Calls API

và developer không cần chỉnh source code để đổi environment endpoint/credential.

# **68. Dependency on DEPLOY-02**

DEPLOY-02 tiếp tục định nghĩa:

- application containerization;

- container networking;

- Compose profiles;

- image build strategy;

- service startup dependencies;

- volumes;

- graceful container shutdown;

- runtime version compatibility.

DEPLOY-01 chỉ định nghĩa local development behavior và requirements.

# **69. Dependency on DEPLOY-03**

DEPLOY-03 tiếp tục định nghĩa:

- thesis demo topology;

- production-like deployment;

- release sequence;

- monitoring;

- backup;

- recovery;

- rollback;

- operational runbooks.

# **70. Out of Scope**

DEPLOY-01 không yêu cầu:

- Kubernetes;

- multi-region architecture;

- autoscaling cluster;

- production CDN design;

- enterprise secrets platform;

- full observability stack;

- multi-cloud deployment;

- production GPU autoscaling;

- high-availability database cluster.

Những nội dung trên không cần thiết cho MVP/thesis unless future requirement xuất hiện.

# **71. Final Development Architecture**

Môi trường local chuẩn có thể tóm tắt:

Vue 3 + Vite + TypeScript

↓

ASP.NET Core

↙ ↓ ↘

PostgreSQL RabbitMQ LocalFileAssetStorage

↓

Python AI Worker

↓

AI Adapter Boundary

Real AI có thể ở local hoặc remote mà không thay application contract.

# **72. Final Principle**

DEPLOY-01 tuân theo bốn nguyên tắc cuối cùng:

> **Application development must not depend on GPU availability.**
>
> **Configuration changes providers; it does not rewrite business logic.**
>
> **Mock and real AI must use the same Job and Context Snapshot pipeline.**
>
> **A clean environment must be reproducible from repository-controlled configuration, migrations and documented startup steps.**

Với các nguyên tắc này, OWNIVERSE có thể phát triển ổn định từ Sprint 0, tích hợp real AI ở các Sprint sau và chuyển sang Thesis Demo environment mà không phải thiết kế lại kiến trúc local development.
