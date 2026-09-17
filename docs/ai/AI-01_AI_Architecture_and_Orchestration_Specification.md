# **AI-01 — AI Architecture & Orchestration Specification**

**System: AI Story Creation Studio  
Document ID: AI-01  
Document Title: AI Architecture & Orchestration Specification  
Version: 0.1  
Status: Draft  
Document Type: Software Architecture / AI System Specification  
Primary Owner: AI Systems Architect / Solution Architect  
Primary Audience: Backend Developer, AI Engineer, Frontend Developer, DevOps Engineer, QA Engineer, System Analyst, Thesis Supervisor**

# **1. Purpose**

**Tài liệu này xác định kiến trúc tích hợp AI của hệ thống AI Story Creation Studio.**

**Mục tiêu chính là thiết lập một ranh giới rõ ràng giữa:**

- **logic nghiệp vụ;**

- **dữ liệu Canon;**

- **Story Core;**

- **cơ chế điều phối AI;**

- **mô hình sinh nội dung;**

- **background job;**

- **asset generation;**

- **và giao diện người dùng.**

**Kiến trúc phải bảo đảm rằng mô hình AI chỉ đóng vai trò generation and reasoning component, không trở thành nơi lưu trữ trạng thái nghiệp vụ hoặc nguồn sự thật của câu chuyện.**

**Thiết kế này nhằm đạt bốn mục tiêu:**

1.  **bảo toàn tính nhất quán của Story Core;**

2.  **cho phép thay đổi AI model mà không ảnh hưởng toàn bộ hệ thống;**

3.  **hỗ trợ các tác vụ AI dài và bất đồng bộ;**

4.  **tạo một nền tảng có thể mở rộng cho text generation, memory processing, consistency checking và image generation.**

# **2. Scope**

**Tài liệu này bao phủ các nội dung sau:**

- **vị trí của AI trong kiến trúc tổng thể;**

- **trách nhiệm của Backend và AI Service;**

- **orchestration của AI request;**

- **model abstraction;**

- **generation job lifecycle;**

- **asynchronous processing;**

- **retry, timeout và failure handling;**

- **structured AI output;**

- **logging và observability;**

- **security boundary;**

- **non-functional requirements;**

- **interface principles giữa các service.**

**Các nội dung sau không thuộc phạm vi tài liệu này:**

- **thuật toán context retrieval chi tiết;**

- **cách tính relevance score;**

- **embedding strategy;**

- **prompt template cụ thể;**

- **memory extraction schema chi tiết;**

- **consistency checking rules chi tiết;**

- **thuật toán image generation;**

- **evaluation metric của mô hình nghiên cứu.**

**Các nội dung trên sẽ được đặc tả trong AI-02 và AI-03.**

# **3. Architectural Context**

**Hệ thống được thiết kế theo nguyên tắc:**

> **Business system owns truth; AI proposes transformations over truth.**

**Điều này có nghĩa là Backend và Story Core phải sở hữu trạng thái chính thức của project. AI chỉ nhận context được Backend hoặc AI Orchestrator cung cấp, sau đó trả về kết quả đề xuất.**

**AI không được tự trực tiếp ghi Canon.**

# **4. High-Level Architecture**

**┌─────────────────────────────────────────────────────────────────┐**

**│ FRONTEND — Vue 3 │**

**│ → Thu nhận thao tác người dùng, gửi request, hiển thị trạng thái │**

**│ generation và kết quả review. │**

**└──────────────────────────────┬──────────────────────────────────┘**

**│ HTTPS**

**▼**

**┌─────────────────────────────────────────────────────────────────┐**

**│ APPLICATION BACKEND — ASP.NET Core │**

**│ → Sở hữu business logic, authorization, Project, Story Core, │**

**│ Canon, Version, Branch và Generation Job metadata. │**

**└─────────────┬──────────────────────┬────────────────────────────┘**

**│ │**

**│ enqueue job │ read/write domain data**

**▼ ▼**

**┌──────────────────────────┐ ┌──────────────────────────────────┐**

**│ QUEUE / JOB SYSTEM │ │ PostgreSQL + pgvector │**

**│ → Điều phối tác vụ dài. │ │ → Lưu Story Core, Canon, state, │**

**└────────────┬─────────────┘ │ metadata và vector index. │**

**│ └──────────────────────────────────┘**

**▼**

**┌─────────────────────────────────────────────────────────────────┐**

**│ AI ORCHESTRATION SERVICE — Python │**

**│ → Chuẩn bị context, xây model request, gọi model adapter, │**

**│ validate output và trả structured result. │**

**└─────────────┬───────────────────────────┬───────────────────────┘**

**│ │**

**▼ ▼**

**┌─────────────────────────────┐ ┌───────────────────────────────┐**

**│ TEXT MODEL ADAPTER │ │ IMAGE MODEL ADAPTER │**

**│ → LLM provider/self-hosted │ │ → Image provider/self-hosted │**

**└─────────────────────────────┘ └───────────────────────────────┘**

**│**

**▼**

**┌─────────────────────────────────────────────────────────────────┐**

**│ OBJECT STORAGE — S3-compatible │**

**│ → Generated image, reference, import và export artifact. │**

**└─────────────────────────────────────────────────────────────────┘**

### **Architectural interpretation**

**Frontend không giao tiếp trực tiếp với LLM hoặc image model.**

**Mọi yêu cầu AI phải đi qua Backend để:**

- **xác thực người dùng;**

- **xác định project;**

- **xác định branch/version;**

- **kiểm tra quyền;**

- **tạo generation job;**

- **ghi lại lifecycle;**

- **và đảm bảo kết quả AI không vượt qua business boundary.**

**AI Service chỉ xử lý những nhiệm vụ có tính AI.**

# **5. Core Architectural Principles**

## **5.1 Separation of Concerns**

**Kiến trúc phải phân tách:**

**Application Backend**

**chịu trách nhiệm về business state.**

**AI Orchestration**

**chịu trách nhiệm điều phối inference.**

**Model**

**chịu trách nhiệm thực hiện generation/inference.**

**Không component nào được đảm nhận toàn bộ ba vai trò trên.**

## **5.2 AI Must Be Stateless with Respect to Canon**

**AI model không được xem như bộ nhớ chính thức của project.**

**Ví dụ không được dựa vào một conversation session kéo dài để duy trì Canon.**

**Mỗi request quan trọng phải có context được xây dựng lại từ dữ liệu hệ thống.**

## **5.3 Model Independence**

**Domain layer không được phụ thuộc vào schema riêng của một nhà cung cấp mô hình.**

**Ví dụ business code không được chứa trực tiếp:**

**OpenAI-specific request**

**Gemini-specific request**

**Claude-specific request**

**Stable Diffusion-specific request**

**Thay vào đó phải thông qua adapter abstraction.**

## **5.3.1 Structured Context Before Prompt**

**Application-level contract cho generation phải là structured, versioned context thay vì raw prompt string.**

**Luồng chuẩn:**

**Domain / Canonical Data**

**↓**

**GenerationContextV1**

**↓**

**Immutable Context Snapshot**

**↓**

**Adapter-specific Prompt / Condition Builder**

**↓**

**Model-specific Input**

**Điều này cho phép StoryDiffusion, Proposed Method (SAMIM) hoặc external image API sử dụng cùng semantic context nhưng render input theo cách riêng của từng model.**

**Raw rendered prompt là provenance/runtime artifact, không phải Canon và không phải application source of truth.**

## **5.4 Human-in-the-loop**

**Mọi kết quả có khả năng thay đổi Canon phải đi qua user review.**

**AI output mặc định là:**

**Proposal / Draft**

**không phải:**

**Canon**

## **5.5 Asynchronous by Default for Expensive Operations**

**Các operation sau nên được thiết kế bất đồng bộ:**

- **scene generation lớn;**

- **project analysis;**

- **memory reconstruction;**

- **consistency scan;**

- **storyboard generation;**

- **multiple-panel generation;**

- **image generation;**

- **export sử dụng AI.**

# **6. Component Responsibilities**

## **6.1 Frontend**

**Frontend chịu trách nhiệm:**

<table>
<colgroup>
<col style="width: 29%" />
<col style="width: 70%" />
</colgroup>
<thead>
<tr class="header">
<th><blockquote>
<p><strong>Responsibility</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Description</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Request Capture</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Thu nhận prompt, command hoặc generation action</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Scope Selection</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Xác định user đang thao tác Scene, Character, Panel...</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Job Visualization</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Hiển thị queued/running/progress/failure</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Result Review</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Hiển thị output AI trước khi Apply/Approve</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Cancellation Request</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Cho phép user yêu cầu hủy nếu operation hỗ trợ</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Version Comparison</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Hiển thị before/after khi cần</strong></p>
</blockquote></th>
</tr>
</thead>
<tbody>
</tbody>
</table>

**Frontend không được tự quyết định Canon transition.**

# **6.2 Application Backend**

**Backend là authoritative application layer.**

**Backend chịu trách nhiệm:**

<table>
<colgroup>
<col style="width: 40%" />
<col style="width: 59%" />
</colgroup>
<thead>
<tr class="header">
<th><blockquote>
<p><strong>Responsibility</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Description</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Authentication</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Xác minh người dùng</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Authorization</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Kiểm tra quyền trên Project</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Domain Validation</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Kiểm tra Project/Scene/Branch tồn tại</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Canon Ownership</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Quản lý Canon lifecycle</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Version Ownership</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Quản lý Version/Branch</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Job Creation</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Tạo Generation Job</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Persistence</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Ghi trạng thái và kết quả</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>AI Request Authorization</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Quyết định request AI hợp lệ</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Approval</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Áp dụng output sau user confirmation</strong></p>
</blockquote></th>
</tr>
</thead>
<tbody>
</tbody>
</table>

## **6.3 AI Orchestration Service**

**AI Orchestration Service chịu trách nhiệm xử lý logic liên quan AI nhưng không sở hữu business state.**

**Các nhiệm vụ gồm:**

**Request Interpretation**

**→ Xác định loại AI task.**

**Context Acquisition**

**→ Lấy dữ liệu cần thiết từ source được cho phép.**

**Prompt / Input Construction**

**→ Tạo model-specific input.**

**Model Invocation**

**→ Gọi model adapter.**

**Output Parsing**

**→ Chuyển model response thành structured object.**

**Output Validation**

**→ Kiểm tra schema và logical constraint sơ bộ.**

**Result Packaging**

**→ Trả kết quả cho Backend.**

# **6.4 Worker**

**Worker thực thi background job.**

**Worker có thể chạy:**

- **text worker;**

- **memory worker;**

- **consistency worker;**

- **image worker.**

**Ở V1, các worker có thể dùng chung codebase nhưng nên giữ ranh giới logic rõ ràng.**

# **6.5 Model Adapter**

**Model Adapter là lớp trung gian giữa orchestration layer và provider.**

**Interface khái niệm:**

**AIModelAdapter**

**│**

**├── GenerateText()**

**├── GenerateStructuredOutput()**

**├── GenerateImage()**

**├── Embed()**

**└── HealthCheck()**

**Không phải mọi adapter đều phải implement toàn bộ operation.**

# **7. Responsibility Boundary**

**Một trong những quyết định kiến trúc quan trọng nhất là phân định trách nhiệm.**

<table>
<colgroup>
<col style="width: 41%" />
<col style="width: 16%" />
<col style="width: 18%" />
<col style="width: 23%" />
</colgroup>
<thead>
<tr class="header">
<th><blockquote>
<p><strong>Operation</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Backend</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>AI Service</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Model</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Kiểm tra user permission</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
<th></th>
<th></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Đọc Canon hiện tại</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
<th></th>
<th></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Chọn context liên quan</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>phối hợp</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
<th></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Build prompt</strong></p>
</blockquote></th>
<th></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
<th></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Generate text</strong></p>
</blockquote></th>
<th></th>
<th></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Validate JSON output</strong></p>
</blockquote></th>
<th></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
<th></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Quyết định output là Canon</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
<th></th>
<th></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Update Character State</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>đề xuất</strong></p>
</blockquote></th>
<th></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Extract potential Event</strong></p>
</blockquote></th>
<th></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Persist Canon Fact</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
<th></th>
<th></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Generate image</strong></p>
</blockquote></th>
<th></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Store image metadata</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>✓</strong></p>
</blockquote></th>
<th></th>
<th></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Store image binary</strong></p>
</blockquote></th>
<th></th>
<th></th>
<th><blockquote>
<p><strong>Object Storage</strong></p>
</blockquote></th>
</tr>
</thead>
<tbody>
</tbody>
</table>

### **Design rule**

**AI Service có thể đề xuất domain changes, nhưng chỉ Backend mới có quyền commit.**

**Ví dụ AI trả:**

**Suggested state transition:**

**Aria.location = "Central Station"**

**Backend không được mặc định ghi ngay.**

**Nó chỉ được áp dụng sau workflow được định nghĩa.**

# **8. AI Task Taxonomy**

**AI task được phân loại theo type để orchestration không phụ thuộc vào text prompt tự do.**

**Ví dụ:**

**STORY_ANALYSIS**

**STORY_GENERATION**

**CHAPTER_GENERATION**

**SCENE_GENERATION**

**TEXT_REWRITE**

**DIALOGUE_GENERATION**

**MEMORY_EXTRACTION**

**STATE_EXTRACTION**

**RELATIONSHIP_EXTRACTION**

**CONSISTENCY_CHECK**

**IMPACT_ANALYSIS**

**STORYBOARD_GENERATION**

**PANEL_SPEC_GENERATION**

**IMAGE_GENERATION**

**IMAGE_REGENERATION**

**EMBEDDING_GENERATION**

**Mỗi task type phải có:**

- **input contract;**

- **output contract;**

- **timeout policy;**

- **retry policy;**

- **model profile;**

- **validation rules.**

# **9. AI Request Lifecycle**

**Luồng tiêu chuẩn:**

**User Action**

**→ Người dùng yêu cầu AI thực hiện một tác vụ.**

**↓**

**Backend Validation**

**→ Xác thực user, project, branch, entity và permission.**

**↓**

**Generation Job Created**

**→ Job được lưu với trạng thái QUEUED.**

**↓**

**Job Enqueued**

**→ Queue chuyển task đến AI Worker.**

**↓**

**Context Preparation**

**→ AI Service xây dựng context phù hợp.**

**↓**

**Model Invocation**

**→ Adapter gọi model tương ứng.**

**↓**

**Output Validation**

**→ Kiểm tra schema, format và constraint.**

**↓**

**Result Persistence**

**→ Backend lưu kết quả thành Draft/Proposal.**

**↓**

**REVIEW_REQUIRED**

**→ Frontend thông báo user xem kết quả.**

**↓**

**User Apply / Reject**

**→ User quyết định sử dụng kết quả.**

**↓**

**Optional Approval**

**→ Nếu nội dung cần trở thành Canon, tiến hành Approve workflow.**

**Luồng này đặc biệt quan trọng vì nó tách:**

**generation**

**khỏi**

**acceptance**

**và tách**

**acceptance**

**khỏi**

**Canon promotion.**

# **10. Generation Job Model**

**Một Generation Job đại diện cho một đơn vị công việc AI có thể theo dõi.**

**Các trường logic tối thiểu:**

<table>
<colgroup>
<col style="width: 38%" />
<col style="width: 61%" />
</colgroup>
<thead>
<tr class="header">
<th><blockquote>
<p><strong>Field</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Description</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>JobId</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Identifier</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>ProjectId</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Project liên quan</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>BranchId</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Branch đang thao tác</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>TargetEntityId</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Scene/Panel/Character...</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>TaskType</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Loại tác vụ AI</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>RequestedBy</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>User tạo job</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Status</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Trạng thái hiện tại</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Progress</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Nếu operation hỗ trợ</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>InputVersion</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Version input</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>OutputVersion</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Version output</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>CreatedAt</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Thời gian tạo</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>StartedAt</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Thời gian chạy</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>CompletedAt</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Thời gian hoàn thành</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>ErrorCode</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Nếu thất bại</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>RetryCount</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Số lần retry</strong></p>
</blockquote></th>
</tr>
</thead>
<tbody>
</tbody>
</table>

# **11. Job State Machine**

**QUEUED**

**→ Job đã được tạo và đang chờ Worker.**

**↓**

**RUNNING**

**→ Worker đã nhận job.**

**↓**

**┌───────────────────────┐**

**│ │**

**▼ ▼**

**REVIEW_REQUIRED FAILED**

**→ Có kết quả → Job không hoàn tất.**

**chờ user.**

**↓**

**┌─────────────┬───────────────┐**

**▼ ▼ ▼**

**APPLIED REJECTED CANCELLED**

**→ User sử → User bỏ → Job bị hủy.**

**dụng.**

**Nếu operation không cần user review, RUNNING có thể đi đến COMPLETED.**

# **12. Job State Requirements**

**AI-JOB-001  
Mỗi tác vụ AI có thời gian xử lý đáng kể phải có Job ID.**

**AI-JOB-002  
Job state phải được persistent; không chỉ lưu trong memory của server.**

**AI-JOB-003  
Refresh browser không được làm mất khả năng theo dõi job.**

**AI-JOB-004  
Một output requiring review không được tự chuyển thành APPLIED.**

**AI-JOB-005  
Job failure không được phá hủy input hoặc version trước đó.**

# **13. Synchronous vs Asynchronous Operations**

**Không phải mọi AI operation đều cần Queue.**

### **Synchronous candidate**

**Có thể dùng synchronous request nếu:**

- **thời gian dự kiến ngắn;**

- **model latency thấp;**

- **request không tạo asset lớn;**

- **retry dễ kiểm soát.**

**Ví dụ:**

**Short rewrite**

**Short metadata suggestion**

**Small classification task**

### **Asynchronous candidate**

**Phải ưu tiên queue nếu:**

- **chạy nhiều model call;**

- **có image generation;**

- **có batch;**

- **có retrieval lớn;**

- **thời gian không ổn định;**

- **output cần progress tracking.**

# **14. Idempotency**

**AI job có chi phí cao, do đó request duplication phải được kiểm soát.**

**Mỗi generation request nên hỗ trợ:**

**Idempotency Key**

**Nếu frontend gửi lại cùng request do network retry, Backend không được vô tình tạo hai job giống hệt nhau nếu request được đánh dấu idempotent.**

# **15. Model Abstraction Layer**

## **15.1 Objective**

**Hệ thống không được phụ thuộc cứng vào một model duy nhất.**

**Model được chọn dựa trên capability, không dựa vào tên provider trong domain logic.**

## **15.2 Model Capability Profile**

**Ví dụ:**

**ModelProfile**

**├── SupportsTextGeneration**

**├── SupportsStructuredOutput**

**├── SupportsLongContext**

**├── SupportsVision**

**├── SupportsImageGeneration**

**├── SupportsReferenceImage**

**├── MaxContextLength**

**├── ExpectedLatency**

**├── CostClass**

**└── DeploymentType**

**Orchestrator chọn model phù hợp dựa trên task.**

# **16. Example Model Routing**

**SCENE_GENERATION**

**→ Long-context capable text model.**

**CONSISTENCY_CHECK**

**→ Reasoning-oriented text model.**

**MEMORY_EXTRACTION**

**→ Structured-output model.**

**IMAGE_GENERATION**

**→ Reference-capable image model.**

**EMBEDDING**

**→ Embedding model.**

**Tên model cụ thể là configuration, không phải domain rule.**

# **17. Model Adapter Contract**

**Adapter phải chuẩn hóa sự khác biệt giữa provider.**

**Ví dụ response từ model A có thể là:**

**content**

**usage**

**finish_reason**

**model B có thể trả format khác.**

**Adapter phải chuyển chúng về một contract nội bộ thống nhất.**

**Khái niệm:**

**AIResponse**

**├── RequestId**

**├── Output**

**├── ModelId**

**├── InputTokens**

**├── OutputTokens**

**├── Latency**

**├── FinishReason**

**└── ProviderMetadata**

**ProviderMetadata không được lan truyền trực tiếp vào domain layer nếu không cần thiết.**

# **18. Structured Output Contract**

**Trong những tác vụ ảnh hưởng logic hệ thống, AI không được trả text tự do nếu Backend cần parse.**

**Ví dụ memory extraction phải trả object có schema xác định.**

**Ví dụ khái niệm:**

**{**

**"events": \[\],**

**"characterStateChanges": \[\],**

**"relationshipChanges": \[\],**

**"canonFacts": \[\]**

**}**

**Schema cụ thể sẽ được định nghĩa tại AI-02.**

# **19. Output Validation**

**Model output phải đi qua ít nhất ba lớp validation.**

**Layer 1 — Syntax Validation**

**→ JSON có hợp lệ không.**

**Layer 2 — Schema Validation**

**→ Có đúng field/type bắt buộc không.**

**Layer 3 — Domain Validation**

**→ Entity ID tồn tại không, transition có hợp lệ không.**

**Layer 3 nên được thực hiện bởi Backend hoặc domain service.**

# **20. Failure Classification**

**Không nên xem mọi lỗi AI là cùng một loại.**

<table>
<colgroup>
<col style="width: 34%" />
<col style="width: 65%" />
</colgroup>
<thead>
<tr class="header">
<th><blockquote>
<p><strong>Error Class</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Ví dụ</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Validation Error</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Input thiếu entity</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Provider Error</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>API model trả lỗi</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Timeout</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Model chạy quá thời gian</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Rate Limit</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Provider từ chối do giới hạn</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Parsing Error</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Output không đúng JSON</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Safety Rejection</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Model từ chối generation</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Infrastructure Error</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Worker/queue/storage lỗi</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Domain Conflict</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Output tham chiếu entity không hợp lệ</strong></p>
</blockquote></th>
</tr>
</thead>
<tbody>
</tbody>
</table>

# **21. Retry Policy**

**Retry phải dựa vào loại lỗi.**

**Ví dụ:**

**Timeout**

**→ Retry với exponential backoff.**

**Rate Limit**

**→ Retry theo retry-after hoặc backoff.**

**Malformed Structured Output**

**→ Có thể retry bằng repair prompt một số lần giới hạn.**

**Domain Validation Failure**

**→ Không retry tự động; cần sửa input/context.**

**Authentication Failure**

**→ Không retry.**

# **22. Retry Requirements**

**AI-ERR-001  
Retry count phải giới hạn.**

**AI-ERR-002  
Không retry vô hạn.**

**AI-ERR-003  
Retry phải được log cùng original Job ID.**

**AI-ERR-004  
Image generation retry không được ghi đè asset thành công trước đó.**

# **23. Timeout Strategy**

**Timeout không nên dùng một giá trị chung cho mọi task.**

**Ví dụ:**

<table>
<colgroup>
<col style="width: 43%" />
<col style="width: 56%" />
</colgroup>
<thead>
<tr class="header">
<th><blockquote>
<p><strong>Task</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Relative Timeout</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Rewrite</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Short</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Scene Generation</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Medium</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Full Story Analysis</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Long</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Image Generation</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Long</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Multi-panel Batch</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Very Long / asynchronous</strong></p>
</blockquote></th>
</tr>
</thead>
<tbody>
</tbody>
</table>

**Các giá trị thực tế sẽ được cấu hình qua environment/configuration.**

# **24. Cancellation**

**Một job có thể có hai loại cancellation:**

**Soft Cancellation**

**Backend đánh dấu user không còn cần kết quả. Worker hoàn thành nhưng kết quả bị bỏ qua.**

**Hard Cancellation**

**Worker/model operation thực sự bị dừng nếu provider hỗ trợ.**

**V1 có thể ưu tiên Soft Cancellation.**

# **25. Concurrency Control**

**Một user có thể tạo nhiều request AI.**

**Hệ thống phải kiểm soát:**

- **duplicate job;**

- **conflicting edit;**

- **stale version;**

- **excessive concurrent generation.**

**Ví dụ:**

**User bắt đầu regenerate Scene Version 3.**

**Trong lúc đó họ chỉnh thành Version 4.**

**Khi AI trả kết quả dựa trên Version 3, hệ thống không được tự áp dụng lên Version 4.**

**Do đó job phải giữ:**

**InputVersion**

**và Backend kiểm tra trước khi Apply.**

# **26. Optimistic Version Validation**

**Quy tắc:**

**Job Input Version = Current Entity Version**

**→ Output có thể Apply.**

**Job Input Version ≠ Current Entity Version**

**→ Mark as STALE_RESULT.**

**User vẫn có thể xem output cũ nhưng không nên được apply âm thầm.**

# **27. Data Ownership**

**Kiến trúc phải quy định rõ nơi lưu dữ liệu.**

<table>
<colgroup>
<col style="width: 47%" />
<col style="width: 52%" />
</colgroup>
<thead>
<tr class="header">
<th><blockquote>
<p><strong>Data</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Owner</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Project</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Backend/PostgreSQL</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Story Core</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Backend/PostgreSQL</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Canon</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Backend/PostgreSQL</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Character State</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Backend/PostgreSQL</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Vector Embedding</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>PostgreSQL/pgvector</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Generation Job</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Backend/PostgreSQL</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Prompt runtime payload</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>AI Service transient storage/log policy</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Generated Image</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Object Storage</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Image Metadata</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Backend/PostgreSQL</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Model provider response metadata</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Observability layer</strong></p>
</blockquote></th>
</tr>
</thead>
<tbody>
</tbody>
</table>

# **28. AI Service State**

**AI Service nên có tính operationally stateless.**

**Điều đó có nghĩa là nếu một AI worker bị restart:**

- **Canon không mất;**

- **project không mất;**

- **job có thể tiếp tục/retry;**

- **history không phụ thuộc vào RAM của AI Service.**

# **29. Prompt and Context Privacy Boundary**

**AI request có thể chứa nội dung project của người dùng.**

**Do đó hệ thống phải bảo đảm:**

- **chỉ lấy context cần thiết;**

- **không gửi toàn bộ project nếu không cần;**

- **không gửi project khác;**

- **không log raw prompt vô hạn;**

- **secret/API key không xuất hiện trong prompt;**

- **access control được xác nhận trước context retrieval.**

# **30. Security Requirements**

**AI-SEC-001  
Frontend không được giữ provider API key.**

**AI-SEC-002  
Provider credential chỉ tồn tại ở server-side secured configuration.**

**AI-SEC-003  
AI Service không được tự bypass authorization bằng cách đọc arbitrary project data.**

**AI-SEC-004  
Job phải gắn với ProjectId và requesting user identity.**

**AI-SEC-005  
Log không được vô tình lưu credential hoặc secret.**

# **31. Observability**

**AI system cần quan sát được ở ba cấp.**

### **Application metrics**

- **số job;**

- **success rate;**

- **failure rate;**

- **retry rate;**

- **average latency.**

### **Model metrics**

- **input token;**

- **output token;**

- **model used;**

- **provider latency;**

- **estimated cost nếu có.**

### **Product metrics**

- **generation accepted;**

- **generation rejected;**

- **regenerate frequency;**

- **consistency warning count.**

**Các metric product sau này cũng có giá trị cho đánh giá luận văn.**

# **32. Logging**

**Mỗi AI request phải có correlation identifier.**

**Ví dụ:**

**RequestId**

**JobId**

**ProjectId**

**TaskType**

**ModelId**

**Attempt**

**Latency**

**Status**

**Không cần log toàn bộ story text trong production log.**

# **33. AI Request Trace**

**Một request nên có khả năng trace:**

**Frontend action**

**→ Backend Request ID**

**→ Generation Job ID**

**→ Worker execution**

**→ Model request**

**→ AI result**

**→ Draft Version**

**→ User Apply/Reject**

**Điều này rất quan trọng khi debug lỗi consistency.**

# **34. Performance Requirements**

**Đối với operation async, hệ thống không bắt buộc hoàn thành ngay nhưng phải phản hồi nhanh rằng job đã được tiếp nhận.**

**AI-NFR-001  
Backend phải trả Job ID sau khi request hợp lệ được enqueue.**

**AI-NFR-002  
UI không bị block trong thời gian AI chạy.**

**AI-NFR-003  
Một job AI thất bại không được làm crash application backend.**

# **35. Scalability**

**Kiến trúc phải cho phép scale độc lập:**

**Web Backend**

**→ scale theo số user/request.**

**Text AI Worker**

**→ scale theo số text job.**

**Image Worker**

**→ scale theo GPU workload.**

**Queue**

**→ buffer workload spike.**

**Đây là lý do AI workload không nên chạy trực tiếp trong ASP.NET request thread.**

# **36. Deployment Model**

**Kiến trúc V1 có thể triển khai như:**

**Application Server**

**├── Vue Frontend**

**├── ASP.NET Core Backend**

**├── PostgreSQL + pgvector**

**├── Queue**

**└── Object Storage integration**

**AI Runtime**

**├── Python AI Service**

**├── Text Worker**

**└── Image Worker / External Image API**

**Trong môi trường thesis, AI Runtime có thể sử dụng:**

- **Google Colab;**

- **cloud GPU;**

- **local GPU;**

- **external AI API;**

**miễn là tuân theo cùng contract.**

# **37. Design Decision: Modular Monolith + AI Services**

**Không cần chia toàn hệ thống thành microservices đầy đủ.**

**Business Backend nên bắt đầu dưới dạng modular monolith.**

**AI workload tách riêng vì:**

1.  **runtime khác nhau;**

2.  **Python thuận lợi cho AI ecosystem;**

3.  **inference có resource profile khác;**

4.  **AI worker có thể cần GPU;**

5.  **lifecycle của AI job khác HTTP business request.**

# **38. Example Sequence — Generate Next Scene**

**User**

**→ Click "Generate Next Scene".**

**Frontend**

**→ POST generation request.**

**Backend**

**→ Validate user/project/branch.**

**→ Create Job J-100.**

**→ Store InputVersion.**

**→ Enqueue task.**

**Worker**

**→ Receive J-100.**

**AI Orchestrator**

**→ Build generation context.**

**→ Select suitable model.**

**→ Invoke model.**

**→ Validate structured output.**

**Backend**

**→ Save Scene Draft Version 5.**

**→ Update Job = REVIEW_REQUIRED.**

**Frontend**

**→ Receive job completion.**

**→ Show Draft Version 5.**

**User**

**→ Apply.**

**Backend**

**→ Set Version 5 as active Draft.**

**User**

**→ Approve later.**

**Backend**

**→ Trigger Canon processing.**

### **Interpretation**

**Generation và Canon update là hai workflow riêng biệt.**

**Điều này ngăn mô hình AI tự thay đổi Story Core chính thức.**

# **39. Example Sequence — Generate Manga Panel**

**User selects Panel 03**

**→ Yêu cầu generate hình ảnh.**

**Backend**

**→ Xác định Panel Spec + Character References.**

**→ Create Image Job.**

**Image Worker**

**→ Nhận Panel Spec.**

**→ AI Orchestrator chuẩn bị image input.**

**→ Image Model tạo ảnh.**

**Object Storage**

**→ Lưu generated asset.**

**Backend**

**→ Lưu Asset metadata + Panel Version.**

**Frontend**

**→ Hiển thị image mới dưới dạng candidate.**

**User**

**→ Select / Reject / Regenerate.**

**Ảnh mới không được tự động trở thành ảnh active nếu UX yêu cầu user review.**

# **40. Functional Requirements**

**AI-ARCH-001  
All AI requests that modify story content shall be mediated by the Application Backend.**

**AI-ARCH-002  
The AI Service shall not directly commit Canon state.**

**AI-ARCH-003  
The system shall support multiple AI model implementations through adapters.**

**AI-ARCH-004  
Long-running AI operations shall support asynchronous execution.**

**AI-ARCH-005  
Each asynchronous generation operation shall be represented by a persistent Generation Job.**

**AI-ARCH-006  
Structured AI tasks shall return schema-valid structured output.**

**AI-ARCH-007  
The system shall preserve the input entity version associated with each generation request.**

**AI-ARCH-008  
The system shall detect stale AI output before applying it to a newer entity version.**

**AI-ARCH-009  
AI failures shall not modify existing Canon or approved content.**

**AI-ARCH-010  
Model-specific implementation details shall not be exposed to domain logic.**

# **41. Non-Functional Requirements**

**AI-NFR-001 — Reliability  
Failure of AI provider must not corrupt domain data.**

**AI-NFR-002 — Maintainability  
Replacing a model provider should require modification primarily in adapter/configuration layers.**

**AI-NFR-003 — Observability  
Every AI job must be traceable from request to output.**

**AI-NFR-004 — Scalability  
AI workers must be independently scalable from application backend.**

**AI-NFR-005 — Recoverability  
Queued/running job metadata must survive backend restart.**

**AI-NFR-006 — Security  
Provider secrets must not be exposed to frontend clients.**

# **42. Acceptance Criteria**

### **AC-AI-01**

**Given a user requests Scene generation,  
when Backend accepts the request,  
then a persistent Generation Job must be created before long-running inference begins.**

### **AC-AI-02**

**Given an AI model returns a new Scene,  
when generation completes,  
then the result must remain Draft/Proposal until explicitly accepted.**

### **AC-AI-03**

**Given AI output was produced from Scene Version 3,  
and the current Scene is already Version 4,  
when the user attempts to apply the output,  
then the system must identify it as stale rather than silently overwriting Version 4.**

### **AC-AI-04**

**Given a model provider becomes unavailable,  
when an AI job fails,  
then existing Project, Canon and approved Version data must remain unchanged.**

### **AC-AI-05**

**Given the configured text model is replaced,  
when the replacement adapter satisfies the internal model contract,  
then Story Core and Frontend must not require structural modification.**

# **43. Architectural Risks**

<table>
<colgroup>
<col style="width: 33%" />
<col style="width: 28%" />
<col style="width: 38%" />
</colgroup>
<thead>
<tr class="header">
<th><blockquote>
<p><strong>Risk</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Impact</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Mitigation</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Model output unpredictable</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Invalid domain update</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Schema validation + user review</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Long inference latency</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Poor UX</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Queue + job status</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Provider dependency</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Vendor lock-in</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Adapter layer</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Duplicate request</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Extra cost/data conflict</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Idempotency</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Stale output</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Overwrite newer edit</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Version check</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Worker failure</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Job lost</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Persistent queue/job metadata</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Model hallucination</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Story inconsistency</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Context + consistency pipeline</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Large context</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>High cost/latency</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Retrieval strategy in AI-02</strong></p>
</blockquote></th>
</tr>
</thead>
<tbody>
</tbody>
</table>

# **44. Design Rationale**

**Kiến trúc này được lựa chọn vì AI generation có bản chất khác với business transaction thông thường.**

**Một HTTP request thông thường có thể:**

**Request**

**→ Validate**

**→ Database**

**→ Response**

**Trong khi AI operation có thể:**

**Request**

**→ Retrieval**

**→ Prompt Construction**

**→ Multiple Model Calls**

**→ Retry**

**→ Parsing**

**→ Validation**

**→ Asset Generation**

**→ User Review**

**Việc đưa toàn bộ quá trình này vào một ASP.NET controller sẽ làm:**

- **request kéo dài;**

- **domain logic phụ thuộc AI provider;**

- **khó scale;**

- **khó retry;**

- **khó quan sát;**

- **khó thay model;**

- **và tăng nguy cơ AI trực tiếp can thiệp Canon.**

**Do đó, orchestration được tách riêng nhưng vẫn để Backend giữ quyền kiểm soát domain.**

# **45. Traceability to Product Goals**

<table>
<colgroup>
<col style="width: 42%" />
<col style="width: 57%" />
</colgroup>
<thead>
<tr class="header">
<th><blockquote>
<p><strong>Product Goal</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Architectural Support</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>User-controlled creation</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Human-in-the-loop lifecycle</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Long story generation</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>AI orchestration + dedicated context layer</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Character consistency</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Structured Story Core input</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Safe Canon management</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Backend owns Canon</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Advanced AI experimentation</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Model abstraction</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Image generation</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Dedicated image job pipeline</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>Reliable UX</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Persistent generation jobs</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>Future model replacement</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Adapter-based integration</strong></p>
</blockquote></th>
</tr>
</thead>
<tbody>
</tbody>
</table>

# **46. Relationship with Subsequent Documents**

**AI-01 thiết lập khung vận hành.**

**Tài liệu tiếp theo sẽ đi sâu vào nội dung AI thực sự sử dụng.**

**AI-01 — Architecture & Orchestration**

**→ AI nằm ở đâu và request vận hành thế nào.**

**↓**

**AI-02 — Context, Memory & Consistency**

**→ AI được cung cấp thông tin gì và hệ thống duy trì consistency thế nào.**

**↓**

**AI-03 — Generation Pipeline**

**→ Context đó được biến thành Story, Scene, Storyboard và Image như thế nào.**

**Ba tài liệu cùng nhau tạo thành AI Application Architecture hoàn chỉnh của hệ thống.**

## **Kết luận kiến trúc**

**Nguyên tắc trung tâm của AI-01 có thể rút gọn thành:**

> **Backend owns truth.  
> AI Service owns orchestration.  
> Models perform inference.  
> Users control acceptance.**
