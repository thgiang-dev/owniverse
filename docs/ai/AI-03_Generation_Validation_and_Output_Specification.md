# **AI-03 — Generation, Validation & Output Specification**

**Document ID:** AI-03  
**Document Type:** AI Application Layer Specification  
**System:** Character-Consistent Long-Range Story Generation  
**Primary Role:** AI Engineer / AI Architect  
**Status:** Draft  
**Related Documents:** AI-01 — AI Pipeline & Orchestration Specification; AI-02 — Context, Memory & Consistency Specification

## **1. Purpose**

Tài liệu này đặc tả cách hệ thống thực hiện các tác vụ sinh nội dung bằng AI, kiểm tra kết quả sinh, quản lý các trường hợp lỗi và chuẩn hóa output trước khi kết quả được sử dụng bởi các thành phần khác của hệ thống.

AI-03 trả lời các câu hỏi chính:

- Một generation request được chuyển thành generation job như thế nào?

- Một job cần những input bắt buộc nào?

- AI service phải trả về output theo cấu trúc nào?

- Hệ thống kiểm tra output bằng những lớp validation nào?

- Khi nào một output được xem là hợp lệ?

- Khi nào hệ thống được tự động retry hoặc regenerate?

- Khi nào cần yêu cầu người dùng quyết định?

- Output AI nào được phép cập nhật Story Core, Scene, Character hoặc Asset?

- Làm thế nào để giữ lại lịch sử generation nhằm hỗ trợ regenerate, comparison và audit?

Tài liệu không định nghĩa chi tiết thuật toán nghiên cứu dùng để duy trì character identity. Phần đó thuộc phạm vi nghiên cứu của luận văn và các thành phần model-specific tương ứng.

# **2. Scope**

AI-03 áp dụng cho các tác vụ AI chính trong quá trình xây dựng một story, bao gồm:

- sinh hoặc mở rộng nội dung story;

- sinh scene description;

- chuẩn hóa dữ liệu character;

- sinh prompt phục vụ image generation;

- sinh hình ảnh cho scene;

- regenerate toàn bộ hoặc một phần output;

- kiểm tra cấu trúc output;

- kiểm tra semantic alignment;

- kiểm tra consistency;

- phát hiện các output không đạt yêu cầu;

- quản lý candidate output;

- lựa chọn output được chấp nhận;

- lưu metadata của generation;

- chuyển output hợp lệ sang các thành phần persistence.

AI-03 tập trung vào **application-level AI workflow**.

# **3. Out of Scope**

Tài liệu này không đặc tả:

- kiến trúc chi tiết của diffusion model;

- cách huấn luyện hoặc fine-tune model;

- thuật toán attention cụ thể;

- loss function;

- phương pháp nghiên cứu mới cho character consistency;

- evaluation protocol dùng để chứng minh contribution của luận văn;

- benchmark học thuật giữa StoryDiffusion, DreamStory, Story-Iter hoặc các baseline khác;

- long-term memory giữa nhiều story độc lập;

- việc duy trì một character vĩnh viễn giữa nhiều phiên sử dụng.

Các nội dung trên thuộc nhánh nghiên cứu hoặc tài liệu AI khác.

# **4. Terminology**

### **4.1 Generation Request**

Yêu cầu ở mức nghiệp vụ gửi từ application tới AI orchestration layer.

Ví dụ:

- Generate Scene 05 image.

- Regenerate Scene 05 with updated expression.

- Generate character profile.

- Generate prompt for Scene 12.

### **4.2 Generation Job**

Đơn vị thực thi cụ thể do AI pipeline tạo ra từ một Generation Request.

Mỗi job phải có identifier riêng và lifecycle độc lập.

### **4.3 Candidate**

Một output do AI tạo ra nhưng chưa được công nhận là output chính thức.

Một generation job có thể tạo một hoặc nhiều candidate.

### **4.4 Accepted Output**

Candidate đã vượt qua các validation bắt buộc và được hệ thống hoặc người dùng lựa chọn làm phiên bản chính thức.

### **4.5 Regeneration**

Thực hiện generation mới dựa trên cùng mục tiêu nghiệp vụ nhưng có thể thay đổi:

- seed;

- generation parameters;

- prompt;

- context;

- user instructions;

- reference asset.

Regeneration không được overwrite lịch sử output cũ.

### **4.6 Retry**

Thực hiện lại cùng một operation do lỗi kỹ thuật hoặc output không thể được xử lý.

Retry không đồng nghĩa với user-requested regeneration.

### **4.7 Validation**

Quá trình kiểm tra xem một output có đủ điều kiện để được sử dụng bởi hệ thống hay không.

### **4.8 Output Contract**

Cấu trúc dữ liệu bắt buộc mà một AI operation phải trả về.

# **5. Design Principles**

## **AI3-PR-01 — Generated data is provisional by default**

Mọi output sinh bởi AI phải được xem là **candidate** cho đến khi hoàn thành các validation bắt buộc.

AI không được trực tiếp ghi output chưa kiểm tra vào canonical story state.

## **AI3-PR-02 — Canonical data must remain distinguishable from generated data**

Hệ thống phải phân biệt rõ:

- dữ liệu do người dùng xác nhận;

- dữ liệu canonical hiện tại;

- dữ liệu được AI đề xuất;

- dữ liệu generation tạm thời.

AI output không được âm thầm thay đổi dữ liệu canonical.

## **AI3-PR-03 — Generation must be reproducible**

Mỗi generation phải lưu đủ metadata để có thể xác định:

- AI operation nào đã chạy;

- input nào được sử dụng;

- context version nào được sử dụng;

- model/provider nào được gọi;

- parameters nào được sử dụng;

- reference assets nào được sử dụng.

## **AI3-PR-04 — Validation must be layered**

Một output không nên chỉ được đánh giá bằng một validation duy nhất.

Validation được chia thành nhiều lớp độc lập.

## **AI3-PR-05 — Regeneration must not destroy history**

Mỗi regeneration tạo một generation attempt mới.

Output cũ vẫn phải được giữ lại để:

- comparison;

- rollback;

- audit;

- debugging;

- research analysis.

## **AI3-PR-06 — AI failures must degrade gracefully**

Một AI service bị lỗi không được làm hỏng Story Core hoặc canonical story state.

# **6. Supported AI Operation Categories**

Hệ thống định nghĩa các operation category chính.

## **6.1 Story Generation**

**Operation:** STORY_GENERATE

Mục tiêu:

Tạo hoặc mở rộng cấu trúc story dựa trên user input.

Output có thể bao gồm:

- title;

- synopsis;

- story premise;

- scene outline;

- character suggestions.

Output này chưa trở thành canonical data cho đến khi được application xử lý theo workflow tương ứng.

## **6.2 Character Generation**

**Operation:** CHARACTER_GENERATE

Mục tiêu:

Tạo structured character definition.

Output có thể gồm:

- name;

- role;

- physical attributes;

- identity-defining attributes;

- default appearance;

- personality description;

- visual description.

Character identity information phải tuân thủ quy tắc được định nghĩa trong AI-02.

## **6.3 Scene Generation**

**Operation:** SCENE_GENERATE

Mục tiêu:

Tạo hoặc mở rộng scene từ story context.

Output có thể gồm:

- scene summary;

- setting;

- participating characters;

- actions;

- emotion;

- dialogue;

- visual description.

## **6.4 Prompt / Model Input Generation**

**Operation:** PROMPT_GENERATE

Mục tiêu:

Chuyển GenerationContextV1 hoặc version context tương ứng thành model-specific prompt, conditioning hoặc request payload mà image-generation adapter yêu cầu.

Prompt generation phải sử dụng structured context đã được AI-02 Context Assembler chuẩn bị.

Raw prompt không phải source of truth và không phải application-level generation contract.

## **6.5 Image Generation**

**Operation:** IMAGE_GENERATE

Mục tiêu:

Sinh visual representation cho một scene.

Input bao gồm tối thiểu:

- immutable GenerationContextV1 / Context Snapshot;

- scene generation context;

- active character context;

- canonical Character References;

- scene-specific Character state;

- style context;

- user instruction;

- generation parameters.

Model-specific prompt/conditioning được Adapter tạo từ structured context và không được frontend cung cấp như source of truth.

## **6.6 Image Regeneration**

**Operation:** IMAGE_REGENERATE

Sinh candidate mới cho một scene đã có output trước đó.

Regeneration có thể được kích hoạt bởi:

- user request;

- validation failure;

- consistency warning;

- prompt modification;

- character modification;

- scene modification.

# **7. Generation Request Contract**

Mọi generation request phải được chuẩn hóa trước khi chuyển tới AI pipeline.

Một request logic phải chứa tối thiểu:

requestId

projectId

storyId

operationType

targetType

targetId

requestedBy

requestedAt

generationIntent

Tùy operation có thể có:

userInstruction

preferredStyle

candidateCount

generationOverrides

sourceVersion

# **8. Generation Job Contract**

Generation Job là runtime representation của một generation operation.

Một job tối thiểu gồm:

jobId

requestId

projectId

storyId

operationType

targetType

targetId

status

contextSnapshotId

inputSnapshotId

modelConfiguration

attemptNumber

createdAt

startedAt

completedAt

error

# **9. Job Status**

Job phải sử dụng lifecycle chuẩn.

QUEUED

↓

PREPARING_CONTEXT

↓

READY

↓

GENERATING

↓

VALIDATING

↓

COMPLETED

Các nhánh lỗi:

GENERATING

↓

FAILED

hoặc:

VALIDATING

↓

REJECTED

hoặc:

GENERATING

↓

RETRY_PENDING

Ngoài ra:

CANCELLED

được sử dụng khi user hoặc system chủ động dừng job.

### **Giải thích luồng**

Khi application gửi yêu cầu generation, job đầu tiên nằm trong hàng đợi. AI orchestration layer sau đó lấy context từ hệ thống, tạo snapshot của input và chuyển job sang trạng thái sẵn sàng. Model được gọi khi job ở GENERATING.

Output sinh ra không lập tức được công nhận mà phải chuyển qua VALIDATING.

Nếu vượt qua các validation bắt buộc, job được đánh dấu COMPLETED.

Nếu generation lỗi kỹ thuật, job chuyển sang FAILED hoặc RETRY_PENDING.

Nếu model tạo được output nhưng output không đạt yêu cầu nghiệp vụ, job chuyển sang REJECTED.

# **10. Generation Input Snapshot**

## **AI3-GEN-01**

Hệ thống MUST tạo immutable input snapshot trước khi gọi model.

Snapshot phải cho biết chính xác generation đó đã sử dụng dữ liệu gì.

## **AI3-GEN-02**

Input snapshot không được tự động thay đổi nếu user chỉnh sửa story trong lúc generation đang chạy.

## **AI3-GEN-03**

Nếu canonical story state thay đổi sau khi job bắt đầu, output của job vẫn phải liên kết với snapshot cũ.

Application có thể cảnh báo rằng output được tạo từ outdated context.

# **11. Model Invocation**

AI service abstraction phải cho phép application tách biệt business workflow khỏi model/provider cụ thể.

Một invocation logic gồm:

GenerationInput

↓

AI Provider Adapter

↓

Model

↓

Raw Model Output

↓

Output Normalizer

↓

Candidate Output

### **Giải thích**

Application không nên xử lý trực tiếp output riêng của từng model.

Provider adapter chịu trách nhiệm chuyển input chuẩn của hệ thống thành định dạng mà model yêu cầu.

Sau khi model trả kết quả, Output Normalizer chuyển kết quả đó về output contract chung trước khi validation bắt đầu.

# **12. Generation Metadata**

Mỗi generation attempt phải ghi lại tối thiểu:

### **Identification**

- jobId;

- attemptId;

- storyId;

- targetId.

### **Model**

- provider;

- model identifier;

- model version nếu có.

### **Input / Contract**

- contextSnapshotId;

- contextSchemaVersion;

- promptTemplateVersion hoặc conditionBuilderVersion nếu applicable;

- adapterVersion;

- methodVersion;

- relevant character IDs;

- scene ID.

Rendered/model-ready prompt hoặc model input payload SHOULD được lưu dưới dạng private provenance khi hợp lý để phục vụ debugging và research reproducibility.

### **Parameters**

Ví dụ:

- seed;

- guidance configuration;

- image dimensions;

- sampling configuration;

- number of inference steps.

Không bắt buộc mọi model phải sử dụng cùng parameter set.

# **13. Candidate Model**

Một candidate phải có lifecycle riêng.

GENERATED

VALIDATING

VALID

INVALID

SELECTED

REJECTED

ARCHIVED

Một job có thể tạo:

Job

├── Candidate A

├── Candidate B

├── Candidate C

└── Candidate D

Một candidate được chọn không đồng nghĩa các candidate còn lại bị xóa.

# **14. Validation Architecture**

Validation pipeline gồm các lớp sau:

Generated Output

↓

V1 — Technical Validation

↓

V2 — Contract Validation

↓

V3 — Semantic Validation

↓

V4 — Consistency Validation

↓

V5 — Safety / Policy Validation

↓

Candidate Decision

### **Giải thích**

Đầu tiên hệ thống kiểm tra output có tồn tại và có thể xử lý về mặt kỹ thuật hay không.

Sau đó kiểm tra cấu trúc output.

Nếu cấu trúc hợp lệ, hệ thống đánh giá output có phản ánh đúng scene hoặc request không.

Tiếp theo là consistency validation, đặc biệt liên quan tới character identity và continuity.

Cuối cùng, các policy cần thiết được áp dụng trước khi candidate được phép xuất hiện trong application.

# **15. V1 — Technical Validation**

Technical validation kiểm tra output ở mức thấp nhất.

## **AI3-VAL-01**

Output MUST tồn tại.

## **AI3-VAL-02**

Output MUST có định dạng hệ thống hỗ trợ.

## **AI3-VAL-03**

Image output MUST có thể decode thành công.

## **AI3-VAL-04**

Image output MUST đáp ứng minimum technical resolution do application configuration quy định.

## **AI3-VAL-05**

Structured text output MUST parse được thành schema tương ứng.

Nếu technical validation thất bại, candidate được đánh dấu INVALID.

# **16. V2 — Contract Validation**

Contract validation đảm bảo AI trả đúng cấu trúc hệ thống yêu cầu.

Ví dụ Scene Generation Contract:

sceneSummary

setting

characters\[\]

actions\[\]

visualDescription

Nếu trường bắt buộc bị thiếu:

contractStatus = FAILED

Hệ thống có thể retry operation nếu lỗi có khả năng do malformed model output.

# **17. V3 — Semantic Validation**

Semantic validation kiểm tra output có thực sự phù hợp với generation intent hay không.

Các tiêu chí có thể bao gồm:

- scene alignment;

- character presence;

- required action presence;

- required object presence;

- setting alignment;

- requested emotion;

- visual style alignment.

## **AI3-VAL-06 — Required Character Presence**

Nếu Scene Context khai báo:

activeCharacters = \[A, B\]

thì output không được vô lý bỏ mất character bắt buộc nếu scene yêu cầu cả hai xuất hiện.

## **AI3-VAL-07 — Forbidden Character Introduction**

AI không được tự ý đưa một recurring character khác vào scene nếu nhân vật đó không thuộc scene context và việc xuất hiện của họ làm thay đổi story semantics.

## **AI3-VAL-08 — Scene Intent Preservation**

Generation không được thay đổi hành động hoặc sự kiện cốt lõi của scene chỉ để tạo hình ảnh dễ hơn.

# **18. V4 — Consistency Validation**

Consistency validation sử dụng các quy tắc và context do AI-02 định nghĩa.

Các nhóm validation chính:

### **Character Identity Consistency**

Kiểm tra character có giữ identity cần thiết qua các scene hay không.

### **Character Separation**

Kiểm tra hai character khác nhau có bị trở nên quá giống nhau hay không.

### **Identity Leakage**

Kiểm tra visual identity của character này có bị truyền sang character khác hay không.

### **Appearance-State Alignment**

Kiểm tra sự thay đổi appearance có phù hợp với scene state hay không.

### **Scene Continuity**

Kiểm tra các yếu tố cần continuity giữa những scene liên quan.

# **19. Validation Severity**

Validation finding được phân thành:

### **INFO**

Không ảnh hưởng đến usability.

### **WARNING**

Output có khả năng sử dụng được nhưng có dấu hiệu lệch.

### **ERROR**

Output không đáp ứng requirement quan trọng.

### **BLOCKING**

Output tuyệt đối không được trở thành accepted output.

# **20. Candidate Validation Result**

Một candidate phải có kết quả validation tổng hợp.

Ví dụ:

validationResult

technical: PASS

contract: PASS

semantic:

status: PASS

score: 0.91

consistency:

status: WARNING

score: 0.74

findings:

\- possible hairstyle drift

overallDecision:

REVIEW_RECOMMENDED

# **21. Candidate Decision**

Hệ thống định nghĩa bốn decision chính:

ACCEPT

REVIEW_RECOMMENDED

REGENERATE_RECOMMENDED

REJECT

## **ACCEPT**

Output đáp ứng các requirement bắt buộc.

## **REVIEW_RECOMMENDED**

Output sử dụng được nhưng có warning.

User được phép xem và quyết định.

## **REGENERATE_RECOMMENDED**

Output không đạt quality threshold nhưng không có lỗi hệ thống nghiêm trọng.

## **REJECT**

Output không được phép sử dụng làm accepted output.

# **22. Automatic Retry Policy**

Retry chỉ nên áp dụng cho failure có tính transient hoặc technical.

Ví dụ:

- provider timeout;

- temporary network failure;

- rate limit;

- malformed response;

- temporary inference failure.

## **AI3-ERR-01**

Retry MUST có giới hạn.

Không job nào được retry vô hạn.

## **AI3-ERR-02**

Retry count phải được lưu trong job metadata.

## **AI3-ERR-03**

Nếu vượt quá retry limit, job chuyển sang FAILED.

# **23. Automatic Regeneration**

Automatic regeneration khác retry.

Regeneration có thể thay đổi generation attempt nhằm tìm candidate khác.

Hệ thống chỉ được tự động regenerate khi policy của operation cho phép.

Ví dụ:

Image generated

↓

Character consistency = ERROR

↓

regenerationAllowed?

↓ yes

Generate new candidate

## **AI3-REG-01**

Automatic regeneration MUST có maximum attempt count.

## **AI3-REG-02**

Nếu nhiều lần regeneration vẫn thất bại, hệ thống phải dừng và yêu cầu user review thay vì chạy vô hạn.

# **24. User-Initiated Regeneration**

User có thể regenerate output hiện tại.

Các mode có thể gồm:

### **Same Instruction**

Giữ nguyên scene/context và tạo candidate mới.

### **Modified Instruction**

User thêm yêu cầu như:

> nhân vật trông lo lắng hơn.

### **Partial Modification**

User chỉ yêu cầu thay đổi một thuộc tính nhất định trong phạm vi system hỗ trợ.

# **25. Regeneration Dependency**

Khi một canonical object thay đổi, hệ thống phải xác định output downstream nào có khả năng outdated.

Ví dụ:

Character appearance changed

↓

Character context version changes

↓

Scenes containing character detected

↓

Existing image generations marked potentially stale

Hệ thống không bắt buộc tự động regenerate tất cả scene.

User cần được thông báo rằng các output cũ sử dụng context version trước đó.

# **26. Output Immutability**

## **AI3-OUT-01**

Một generated candidate sau khi lưu phải được xem là immutable artifact.

Không sửa trực tiếp candidate cũ.

## **AI3-OUT-02**

Nếu cần thay đổi, hệ thống tạo candidate hoặc version mới.

# **27. Accepted Output**

Khi một candidate được lựa chọn:

candidate.status = SELECTED

Scene hoặc entity tương ứng sẽ lưu reference tới candidate đó.

Ví dụ:

Scene

└── selectedImageCandidateId

Output image không cần được copy hoặc overwrite thành một object mới.

# **28. Canonical State Update Rules**

AI output không tự động có quyền cập nhật mọi dữ liệu trong Story Core.

Các field được chia thành ba loại.

## **28.1 AI-Proposable**

AI được phép đề xuất giá trị.

Ví dụ:

- generated description;

- scene summary;

- suggested visual prompt.

## **28.2 AI-Updatable with Confirmation**

AI có thể đề xuất thay đổi nhưng cần user hoặc workflow xác nhận.

Ví dụ:

- character canonical appearance;

- permanent character trait;

- story fact;

- relationship between characters.

## **28.3 AI-Read-Only**

AI chỉ được đọc.

Ví dụ:

- immutable object ID;

- ownership;

- user-selected canonical identity reference;

- manually locked attributes.

# **29. Locked Attributes**

User hoặc system có thể lock một số canonical attribute.

Ví dụ:

eyeColor = blue

locked = true

AI không được thay đổi thuộc tính này trong canonical state.

Nếu generated output xung đột với locked attribute:

consistencyFinding = LOCKED_ATTRIBUTE_MISMATCH

# **30. Output Versioning**

Generation history được tổ chức theo target.

Ví dụ:

Scene 12

Generation 1

└── Candidate A

Generation 2

└── Candidate B

Generation 3

├── Candidate C

└── Candidate D

Một candidate được selected tại một thời điểm nhưng user có thể quay lại candidate trước đó.

# **31. Asset Storage**

Image generation output phải được lưu thông qua Asset Storage Layer.

Database không nên lưu binary image trực tiếp trong các domain entity nếu kiến trúc storage đã tách riêng.

Domain object chỉ lưu reference tới asset.

Ví dụ:

candidateId

assetId

previewAssetId

# **32. Provenance**

Mỗi generated artifact phải có provenance.

Tối thiểu phải truy được:

Artifact

↓

Candidate

↓

Generation Attempt

↓

Generation Job

↓

Input Snapshot

↓

Context Snapshot

Điều này đặc biệt quan trọng cho:

- debugging;

- reproducibility;

- thesis experiments;

- consistency analysis.

# **33. Failure Classification**

Generation failure được phân loại thành:

### **TECHNICAL_FAILURE**

Provider hoặc infrastructure lỗi.

### **INPUT_FAILURE**

Input không hợp lệ hoặc thiếu.

### **CONTEXT_FAILURE**

Không thể build context hợp lệ.

### **GENERATION_FAILURE**

Model không tạo được output.

### **VALIDATION_FAILURE**

Output được tạo nhưng không đạt validation.

### **POLICY_FAILURE**

Output vi phạm policy.

### **USER_CANCELLED**

User chủ động cancel.

# **34. Error Contract**

Error object tối thiểu gồm:

errorCode

category

message

retryable

userActionRequired

technicalDetailsReference

occurredAt

Technical details không nhất thiết được trả trực tiếp cho frontend.

# **35. Observability**

Hệ thống phải thu thập các metrics cần thiết cho AI operations.

Tối thiểu:

- number of jobs;

- success rate;

- failure rate;

- retry rate;

- regeneration rate;

- validation rejection rate;

- average generation latency;

- model/provider usage.

# **36. Research-Support Metadata**

Do application đồng thời là môi trường minh họa cho luận văn, generation pipeline nên giữ metadata đủ để phục vụ phân tích sau này.

Ví dụ:

- recurring character IDs;

- character count trong scene;

- scene position;

- distance từ lần xuất hiện trước;

- generation attempt;

- consistency validation results.

Tuy nhiên dữ liệu phục vụ nghiên cứu không được làm thay đổi application behavior nếu không được quy định bởi system specification.

# **37. Functional Requirements**

## **AI3-FR-01**

System SHALL tạo Generation Job cho mỗi AI operation cần thực thi bất đồng bộ.

## **AI3-FR-02**

System SHALL tạo immutable input snapshot trước model invocation.

## **AI3-FR-03**

System SHALL lưu model và generation metadata cho mỗi attempt.

## **AI3-FR-04**

System SHALL chuẩn hóa raw model output trước validation.

## **AI3-FR-05**

System SHALL chạy tất cả mandatory validation trước khi candidate có thể được selected.

## **AI3-FR-06**

System SHALL giữ generation history sau regeneration.

## **AI3-FR-07**

System SHALL hỗ trợ nhiều candidate cho cùng target.

## **AI3-FR-08**

System SHALL cho phép user lựa chọn candidate hợp lệ làm selected output.

## **AI3-FR-09**

System SHALL không cho invalid candidate trở thành canonical selected output.

## **AI3-FR-10**

System SHALL lưu provenance của generated artifact.

## **AI3-FR-11**

System SHALL giới hạn automatic retry.

## **AI3-FR-12**

System SHALL giới hạn automatic regeneration.

## **AI3-FR-13**

System SHALL đánh dấu output có khả năng stale khi canonical dependency thay đổi.

## **AI3-FR-14**

System SHALL bảo vệ locked canonical attributes khỏi việc bị AI tự động ghi đè.

## **AI3-FR-15**

System SHALL phân biệt retry với regeneration.

# **38. Non-Functional Requirements**

## **AI3-NFR-01 — Traceability**

Mỗi generated output phải truy ngược được tới generation input và context tương ứng.

## **AI3-NFR-02 — Recoverability**

AI failure không được làm mất canonical story state.

## **AI3-NFR-03 — Extensibility**

Generation architecture phải cho phép thay model/provider mà không yêu cầu viết lại business workflow chính.

## **AI3-NFR-04 — Auditability**

Generation, validation, selection và regeneration phải có audit history.

## **AI3-NFR-05 — Idempotency**

Các operation có nguy cơ bị gửi lại do network retry phải có cơ chế tránh vô tình tạo duplicated canonical updates.

# **39. Main Flow — Scene Image Generation**

### **Preconditions**

- Story tồn tại.

- Scene tồn tại.

- Scene có đủ dữ liệu bắt buộc.

- Relevant character data tồn tại.

- Context assembler hoạt động thành công.

### **Main Flow**

1.  User yêu cầu generate image cho scene.

2.  Application tạo Generation Request.

3.  AI Orchestrator tạo Generation Job.

4.  AI-02 Context Layer xây Scene Generation Context.

5.  Hệ thống tạo immutable context/input snapshot.

6.  Prompt Generation tạo prompt cuối.

7.  Image Generation Service được gọi.

8.  Model trả raw image output.

9.  Output được normalize thành Candidate.

10. Technical validation được thực hiện.

11. Semantic validation được thực hiện.

12. Consistency validation được thực hiện.

13. Candidate nhận validation result.

14. Candidate hợp lệ được gửi về application.

15. User hoặc workflow lựa chọn candidate.

16. Candidate trở thành selected output của scene.

17. Generation history và provenance được lưu.

# **40. Alternate Flow — Validation Failure**

Tại bước validation:

1.  Validator phát hiện failure.

2.  Candidate được đánh dấu INVALID hoặc REGENERATE_RECOMMENDED.

3.  System kiểm tra regeneration policy.

4.  Nếu còn attempt:

    - tạo generation attempt mới;

    - sinh candidate mới;

    - validation lại.

5.  Nếu hết attempt:

    - dừng automatic generation;

    - job chuyển trạng thái phù hợp;

    - application yêu cầu user review.

# **41. Alternate Flow — Context Changed During Generation**

1.  Job sử dụng Context Version 12.

2.  User sửa Character A.

3.  Canonical Character Context trở thành Version 13.

4.  Generation đang chạy vẫn tiếp tục với Version 12.

5.  Output hoàn thành.

6.  System phát hiện current canonical version khác snapshot version.

7.  Candidate được đánh dấu:

CONTEXT_OUTDATED

8.  User có thể:

    - giữ candidate;

    - regenerate bằng context mới.

# **42. Acceptance Criteria**

### **AC-AI3-01**

Given một scene hợp lệ,  
when user yêu cầu generation,  
then hệ thống tạo generation job có unique identifier.

### **AC-AI3-02**

Given generation job đã bắt đầu,  
when story data thay đổi,  
then input snapshot của job không thay đổi.

### **AC-AI3-03**

Given model trả output malformed,  
when contract validation chạy,  
then candidate không được trở thành selected output.

### **AC-AI3-04**

Given candidate vượt qua mandatory validation,  
when user chọn candidate,  
then scene reference được cập nhật tới candidate đó.

### **AC-AI3-05**

Given một selected candidate đã tồn tại,  
when user regenerate,  
then output cũ vẫn tồn tại trong generation history.

### **AC-AI3-06**

Given generation gặp transient provider error,  
when retry policy cho phép,  
then system retry nhưng không vượt quá configured maximum.

### **AC-AI3-07**

Given canonical character attribute bị lock,  
when AI output đề xuất giá trị khác,  
then canonical attribute không được tự động thay đổi.

### **AC-AI3-08**

Given output được sinh từ context version cũ,  
when current context đã thay đổi,  
then system có thể xác định candidate là stale/outdated.

### **AC-AI3-09**

Given một generated image,  
when developer kiểm tra provenance,  
then có thể truy ngược tới generation job, attempt, input snapshot và context snapshot tương ứng.

# **43. Dependencies**

AI-03 phụ thuộc trực tiếp vào:

### **AI-01 — AI Pipeline & Orchestration Specification**

Cung cấp:

- orchestration lifecycle;

- AI operation routing;

- job execution;

- provider interaction.

### **AI-02 — Context, Memory & Consistency Specification**

Cung cấp:

- context assembly;

- Story Core context;

- character context;

- scene context;

- identity memory;

- consistency rules;

- context versioning.

### **Data Specification**

Cung cấp persistence model cho:

- generation jobs;

- candidates;

- assets;

- versions;

- snapshots.

### **API Specification**

Cung cấp endpoint/event contract cho:

- generation request;

- progress;

- result;

- retry;

- regenerate;

- candidate selection.

# **44. Assumptions**

- Generation có thể mất đủ lâu để cần asynchronous processing.

- Một target có thể có nhiều generation attempts.

- Một generation có thể trả nhiều candidate.

- AI output không được coi là hoàn toàn đáng tin cậy.

- Validation không đảm bảo tuyệt đối chất lượng nghệ thuật.

- Một số quality decision cuối cùng vẫn cần người dùng.

- Model/provider có thể thay đổi trong quá trình phát triển.

- Character consistency mechanism có thể thay đổi khi nghiên cứu luận văn tiến triển.

# **45. Traceability**

| **Requirement** | **Related Area**               |
|-----------------|--------------------------------|
| AI3-FR-01       | AI-01 Job Orchestration        |
| AI3-FR-02       | AI-02 Context Snapshot         |
| AI3-FR-03       | Data / Generation Metadata     |
| AI3-FR-04       | AI Provider Adapter            |
| AI3-FR-05       | Validation Pipeline            |
| AI3-FR-06       | Asset & Versioning             |
| AI3-FR-07       | Candidate Management           |
| AI3-FR-08       | Product / UX Generation Review |
| AI3-FR-09       | Validation                     |
| AI3-FR-10       | Data Provenance                |
| AI3-FR-11       | AI-01 Retry Policy             |
| AI3-FR-12       | Generation Policy              |
| AI3-FR-13       | AI-02 Context Versioning       |
| AI3-FR-14       | AI-02 Canonical Data Rules     |
| AI3-FR-15       | AI-01 Job Lifecycle            |

# **46. Relationship Between AI-01, AI-02 and AI-03**

Ba tài liệu AI tạo thành một chuỗi trách nhiệm rõ ràng:

AI-01

AI Pipeline & Orchestration

↓

"AI task nào cần chạy và chạy theo workflow nào?"

AI-02

Context, Memory & Consistency

↓

"AI cần biết những gì để thực hiện task đó?"

AI-03

Generation, Validation & Output

↓

"AI sinh ra cái gì, kiểm tra nó thế nào,

và kết quả nào được phép đi tiếp?"

Nói bằng lời:

**AI-01 điều phối công việc.**

**AI-02 chuẩn bị trí nhớ và ngữ cảnh.**

**AI-03 thực hiện generation, kiểm tra kết quả và quản lý output.**

Ba tài liệu cùng nhau tạo thành specification hoàn chỉnh cho **AI Application Layer**, trong khi thuật toán nghiên cứu character consistency vẫn có thể tiếp tục phát triển độc lập mà không phá vỡ contract của application.

# **47. Final Architectural Rule**

Quy tắc quan trọng nhất của AI-03 là:

> **AI generates proposals; the system owns state.**

Model có thể tạo story, scene, prompt hoặc image, nhưng model không phải nguồn dữ liệu chuẩn của hệ thống.

Canonical state phải luôn được kiểm soát bởi application thông qua:

**Generation → Validation → Selection → Persistence.**

Điều này giúp hệ thống vừa khai thác khả năng sáng tạo của AI, vừa duy trì được consistency, traceability, recoverability và khả năng thay thế model trong tương lai.
