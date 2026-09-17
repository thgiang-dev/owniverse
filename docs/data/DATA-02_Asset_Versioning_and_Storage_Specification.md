# **DATA-02 — Asset, Versioning & Storage Specification**

**Document ID:** DATA-02  
**Document Type:** Asset, Versioning & Storage Specification  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Primary Role:** Data Architect / Storage Architect  
**Status:** Draft v1  
**Parent Documents:** ARCH-01, ARCH-02, DOMAIN-01, DOMAIN-02, DATA-01  
**Related Documents:** API-01, API-02, AI-01, AI-02, AI-03, QA-01, QA-02

## **1. Purpose**

Tài liệu này định nghĩa cách OWNIVERSE lưu trữ và quản lý những dữ liệu cần **lịch sử, khả năng tái hiện, provenance và file storage**.

DATA-01 đã trả lời:

> Database có những entity nào và chúng quan hệ với nhau ra sao?

DATA-02 trả lời:

> **Khi entity thay đổi, AI generate output, file ảnh được tạo và context thay đổi theo thời gian thì hệ thống lưu tất cả những thứ đó như thế nào mà không mất lịch sử?**

Tài liệu tập trung vào:

- Asset Storage;

- Entity Versioning;

- Context Snapshot;

- Snapshot Dependencies;

- Generation Provenance;

- Stale Detection;

- Reference Versioning;

- Asset Lifecycle;

- Storage Security;

- Retention;

- Cleanup;

- Reproducibility.

# **2. Core Principle**

Nguyên tắc trung tâm:

> **Never overwrite the evidence used to create an AI output.**

Nếu Candidate được tạo bằng:

Character A v3

Scene 12 v4

StoryCore v7

thì dù hiện tại Character đã lên v8, hệ thống vẫn phải biết Candidate đó từng được tạo bằng v3.

# **3. Data Categories**

DATA-02 chia dữ liệu thành bốn nhóm lớn:

Canonical Current State

↓

Version History

↓

Generation Snapshot

↓

Generated Assets & Provenance

Ý nghĩa:

**Canonical Current State  
**là dữ liệu hiện tại của Story.

**Version History  
**là lịch sử dữ liệu đã thay đổi.

**Generation Snapshot  
**là bộ context AI thực sự sử dụng.

**Generated Assets & Provenance  
**là kết quả AI cùng toàn bộ nguồn gốc của nó.

# **4. Asset**

## **4.1 Definition**

**Asset** là một file vật lý hoặc digital resource được OWNIVERSE quản lý.

Ví dụ:

- generated image;

- uploaded Character reference;

- Story cover;

- thumbnail;

- preview image;

- exported page;

- future audio/video asset.

# **5. Asset Metadata vs Physical File**

Asset được chia thành:

Database

→ Asset Metadata

Object Storage

→ Physical File

Database không nên giữ binary image lớn trong core tables.

# **6. Recommended Asset Table**

Logical table:

assets

Field chính:

asset_id

project_id

asset_type

source_type

storage_provider

storage_bucket

storage_key

original_file_name

mime_type

file_size_bytes

width

height

checksum

status

created_at

archived_at

# **7. Asset Type**

asset_type có thể gồm:

CHARACTER_REFERENCE

GENERATED_IMAGE

PROJECT_COVER

THUMBNAIL

SCENE_PREVIEW

EXPORT

OTHER

# **8. Asset Source**

source_type cho biết file đến từ đâu:

USER_UPLOAD

AI_GENERATED

SYSTEM_GENERATED

IMPORT

Điều này khác với asset_type.

Ví dụ:

asset_type = CHARACTER_REFERENCE

source_type = AI_GENERATED

hoặc:

asset_type = CHARACTER_REFERENCE

source_type = USER_UPLOAD

# **9. Storage Key**

Database không nên chỉ lưu public URL.

Nên lưu:

storage_provider

storage_bucket

storage_key

Ví dụ:

provider = S3

bucket = owniverse-assets

key = projects/{projectId}/images/{assetId}.png

URL truy cập có thể được tạo động.

# **10. Why Not Store Permanent Public URL**

Vì URL có thể thay đổi khi:

- chuyển provider;

- đổi CDN;

- chuyển bucket;

- bật signed URL;

- đổi domain.

storage_key ổn định hơn public URL.

# **11. Object Storage**

Production-like environment nên sử dụng object storage.

Ví dụ:

- S3-compatible storage;

- MinIO;

- cloud object storage.

Development có thể dùng local file storage.

Application interface không nên phụ thuộc trực tiếp provider.

# **12. Asset Storage Path**

Có thể tổ chức logical key:

projects/

{projectId}/

characters/

scenes/

generated/

thumbnails/

Tuy nhiên asset_id vẫn là identity chính.

Folder không phải domain relationship.

# **13. Asset Immutability**

Một generated Asset sau khi hoàn thành phải được xem như **immutable**.

Không:

Candidate A image

→ overwrite same file

Khi regenerate:

Candidate B

→ New Asset

# **14. Why Asset Immutability Matters**

Nếu overwrite ảnh cũ:

- history bị phá;

- Candidate cũ thay đổi;

- research result không reproducible;

- provenance mất ý nghĩa.

Do đó mỗi generated output phải giữ file riêng.

# **15. Checksum**

Asset nên lưu checksum, ví dụ SHA-256.

Mục đích:

- phát hiện corruption;

- detect duplicate khi cần;

- verify file integrity;

- support reproducibility.

# **16. Asset Status**

Có thể gồm:

PENDING

ACTIVE

ARCHIVED

FAILED

DELETED

DELETED chỉ nên dùng khi file vật lý thực sự đã được cleanup theo policy.

# **17. Asset Creation Flow**

Generation image:

AI Worker

↓

Temporary File

↓

Upload Object Storage

↓

Create Asset Record

↓

Create Candidate

Candidate không nên được xem là hoàn chỉnh nếu Asset upload chưa thành công.

# **18. Character Reference Asset**

Quan hệ:

Character

↓

CharacterReference

↓

Asset

Character không trực tiếp chứa image binary.

# **19. Candidate Asset**

Quan hệ:

Candidate

↓

Asset

Một generated Candidate image thường tham chiếu một Asset.

# **20. Thumbnail**

Thumbnail không cần là Asset độc lập trong mọi trường hợp.

Có hai lựa chọn:

Asset

├── Original

└── Thumbnail Variant

hoặc:

Original Asset

Thumbnail Asset

Đề xuất MVP:

> dùng Asset Variant thay vì tạo domain asset hoàn toàn độc lập cho mọi thumbnail.

# **21. Asset Variant**

Logical table optional:

asset_variants

Field:

asset_variant_id

asset_id

variant_type

storage_key

width

height

file_size_bytes

created_at

Ví dụ:

THUMBNAIL

PREVIEW

WEB_OPTIMIZED

# **22. Versioning Principle**

Canonical entities thay đổi theo thời gian nhưng historical state phải được giữ.

Ta sử dụng:

> **Current Entity + Immutable Version Records**

# **23. Current Entity vs Version**

Ví dụ:

characters

giữ trạng thái hiện tại.

Trong khi:

character_versions

giữ lịch sử.

# **24. Character Version**

Logical table:

character_versions

Field chính:

character_version_id

character_id

version_number

snapshot_json

change_source

change_summary

created_by_user_id

created_at

# **25. Character Version Snapshot**

snapshot_json giữ canonical Character state tại version đó.

Ví dụ:

{

"name": "Aira",

"stableIdentity": {

"hairColor": "black",

"eyeColor": "blue"

},

"baseAppearance": {

"hairStyle": "long"

},

"personality": {

"traits": \["quiet", "curious"\]

}

}

# **26. Why Full Version Snapshot**

Có hai cách:

Full Snapshot

hoặc:

Diff

Đối với thesis/MVP, đề xuất:

> **Full Snapshot per Version**

Lý do:

- dễ đọc;

- dễ debug;

- dễ restore;

- dễ reproducibility;

- dễ dùng trong Context Snapshot.

Dung lượng metadata nhỏ hơn rất nhiều so với image assets nên trade-off này hợp lý.

# **27. Scene Version**

Logical table:

scene_versions

Field:

scene_version_id

scene_id

version_number

snapshot_json

change_source

change_summary

created_by_user_id

created_at

Snapshot có thể chứa:

- title;

- description;

- location;

- narrative content;

- SceneCharacter state references;

- creative instruction.

# **28. Story Core Version**

Logical table:

story_core_versions

Field:

story_core_version_id

story_core_id

version_number

snapshot_json

change_source

change_summary

created_by_user_id

created_at

# **29. World / Relationship Versioning**

Không bắt buộc tạo version table riêng cho mọi entity ngay từ MVP.

Versioning priority:

StoryCore

Character

Scene

Sau đó có thể mở rộng cho:

World

Location

Relationship

StoryFact

nếu use case thực tế cần.

# **30. Version Number**

Trong mỗi entity:

version_number

bắt đầu:

1

sau đó tăng:

2

3

4

...

Constraint:

UNIQUE(entity_id, version_number)

# **31. Current Version Reference**

Core table có thể giữ:

current_version_number

hoặc:

current_version_id

Đề xuất:

> dùng current_version_number hoặc revision field trên current row, còn version table giữ lịch sử.

Không bắt buộc current row phải FK trực tiếp ngược sang version record.

# **32. Version Creation Rule**

Khi canonical update thành công:

BEGIN

Validate revision

Create new version snapshot

Update current entity

Increment revision/version

COMMIT

Version và current state phải thay đổi atomically.

# **33. First Version**

Ngay khi Character được tạo:

Character

→ CharacterVersion 1

Không đợi tới lần edit thứ hai mới bắt đầu lưu version.

# **34. Restore**

Nếu hiện tại:

Character v8

user muốn quay lại trạng thái của:

v3

thì:

v9 = copy of v3

Không biến current version thành v3 trực tiếp.

# **35. Version Immutability**

Version record:

CharacterVersion 3

không được edit sau khi tạo.

Nếu phát hiện sai canonical data:

tạo version mới.

# **36. Change Source**

Mỗi version cần biết nguồn thay đổi.

Possible values:

USER_EDIT

AI_PROPOSAL_ACCEPTED

RESTORE

SYSTEM_ACTION

IMPORT

# **37. Change Summary**

Optional:

"Changed hair color from black to silver"

phục vụ history UI.

Không dùng Change Summary làm source of truth.

# **38. Context Snapshot**

## **38.1 Definition**

**Context Snapshot** là bộ dữ liệu immutable mà AI thực sự được cung cấp cho một generation operation.

Đây là một trong những entity quan trọng nhất đối với luận văn.

# **39. Why Context Snapshot Exists**

Nếu chỉ lưu:

job → CharacterId

thì vài ngày sau Character đã thay đổi.

Ta không còn biết:

> AI lúc đó thấy Character như thế nào?

Snapshot giải quyết vấn đề này.

# **40. Context Snapshot Table**

Logical table:

context_snapshots

Field:

context_snapshot_id

project_id

story_id

target_type

target_id

snapshot_type

snapshot_data_json

schema_version

created_at

Snapshot không có:

updated_at

vì nó immutable.

# **41. Snapshot Data**

Ví dụ:

{

"storyCore": {

"version": 7

},

"scene": {

"id": "...",

"version": 4

},

"characters": \[

{

"id": "A",

"version": 3

},

{

"id": "B",

"version": 6

}

\]

}

# **42. Snapshot Does Not Need Full Binary**

Context Snapshot không copy Character Reference image binary vào database.

Nó lưu reference tới Asset/version.

# **43. Snapshot Dependency**

Thay vì chỉ nhét mọi dependency vào JSON, nên có relational dependency table để query stale output.

Logical table:

context_snapshot_dependencies

# **44. Snapshot Dependency Fields**

dependency_id

context_snapshot_id

entity_type

entity_id

version_number

dependency_role

created_at

# **45. Dependency Role**

Ví dụ:

STORY_CORE

TARGET_SCENE

ACTIVE_CHARACTER

CHARACTER_REFERENCE

LOCATION

RELEVANT_FACT

STYLE

# **46. Example Snapshot Dependencies**

Snapshot S100

phụ thuộc:

StoryCore v7

Scene 12 v4

Character A v3

Character B v6

CharacterReference A v2

# **47. Why Dependency Table Matters**

Khi:

Character A v3

→ v4

ta có thể query:

> Context Snapshot nào từng dùng Character A v3?

Từ đó biết Candidate nào bị ảnh hưởng.

# **48. Generation Provenance**

Provenance chain đầy đủ:

Candidate

↓

GenerationAttempt

↓

GenerationJob

↓

ContextSnapshot

↓

SnapshotDependencies

↓

Entity Versions

# **49. Asset Provenance**

Với image Candidate:

Candidate

↓

Asset

Asset có thể lưu:

source_job_id

source_attempt_id

nhưng không bắt buộc nếu Candidate đã cung cấp chain.

Tránh duplicate dependency nếu không cần.

# **50. Candidate Provenance**

Candidate phải giữ:

candidate_id

job_id

attempt_id

asset_id

Từ đó truy ngược đủ.

# **51. Model Provenance**

Generation Attempt cần lưu:

model_identifier

model_version

configuration_json

Ví dụ:

model_identifier = proposed-method

model_version = v0.3

# **52. Research Configuration**

configuration_json có thể chứa:

{

"seed": 12345,

"steps": 30,

"guidanceScale": 7.5,

"scheduler": "...",

"experiment": "EXP-04"

}

Chỉ lưu những config cần cho reproducibility.

# **53. Generation Input / Prompt Provenance**

Context Snapshot giữ structured source input. Ngoài Snapshot, hệ thống SHOULD giữ model-ready prompt, conditioning metadata hoặc request payload đã được Adapter render nếu việc lưu trữ đó cần cho debugging/research reproducibility.

Đề xuất logical field:

input_payload_json

trong Attempt hoặc một Generation Input record riêng nếu payload lớn.

Provenance metadata tối thiểu nên cho phép lưu:

- contextSchemaVersion;

- promptTemplateVersion hoặc conditionBuilderVersion;

- adapterVersion;

- modelIdentifier;

- modelVersion;

- methodVersion;

- seed nếu applicable.

Rendered prompt không thay thế Context Snapshot và không trở thành source of truth.

# **54. Prompt Security**

Nếu Story private:

prompt/context cũng được xem là private project data.

Không log prompt vào public observability system mặc định.

# **55. Stale Candidate**

Candidate không nhất thiết có một boolean đơn giản:

is_stale

vì stale có nhiều mức.

Đề xuất:

stale_status

và:

stale_reason_json

# **56. Stale Status**

Possible values:

CURRENT

POTENTIALLY_STALE

REVIEW_RECOMMENDED

REGENERATION_RECOMMENDED

INVALIDATED

# **57. Stale Evaluation**

Ví dụ Candidate phụ thuộc:

Character A v3

hiện tại:

Character A v4

Hệ thống kiểm tra thay đổi v3 → v4.

Nếu chỉ đổi:

Biography punctuation

Candidate có thể vẫn:

CURRENT

Nếu đổi:

Hair color

thì:

REGENERATION_RECOMMENDED

# **58. Simple MVP Stale Strategy**

Không cần xây dependency reasoning quá thông minh ngay.

MVP có thể dùng:

Referenced Entity Version changed

→ POTENTIALLY_STALE

Sau đó user review.

Advanced version mới phân loại field-level impact.

# **59. Stale Evaluation Record**

Optional logical table:

candidate_stale_evaluations

Field:

evaluation_id

candidate_id

status

reason_json

evaluated_at

MVP có thể lưu trực tiếp status trên Candidate để đơn giản.

# **60. Selected Candidate and Stale**

Nếu Candidate đang selected rồi trở thành stale:

không tự động bỏ selection.

Scene vẫn giữ Candidate.

UI hiển thị warning:

> Generated with older character data.

# **61. Why Not Auto-Regenerate**

Auto-regeneration có thể:

- tốn GPU;

- làm thay đổi artwork user thích;

- phá creative decision;

- tạo chi phí không kiểm soát.

Do đó stale chỉ tạo decision point.

# **62. Reference Versioning**

Character Reference cũng thay đổi theo thời gian.

Ví dụ:

Reference v1:

long hair

Scene 25:

canonical haircut event

Reference v2:

short hair

# **63. Character Reference Version**

Logical extension:

character_reference_versions

hoặc đơn giản hơn:

mỗi CharacterReference là immutable reference record mới.

Đề xuất MVP:

> CharacterReference record không overwrite Asset.

Thay reference → tạo CharacterReference mới và archive reference cũ.

# **64. Reference Effective Range**

Để hỗ trợ temporal appearance:

effective_from_scene_id

effective_until_scene_id

có thể được thêm vào CharacterReference.

# **65. Example**

Reference A1

Scenes 1–24

Reference A2

Scenes 25+

AI Context Builder chọn reference phù hợp với Scene.

# **66. Current Reference**

is_primary chỉ có nghĩa:

> reference mặc định hiện tại.

Nó không có nghĩa reference đó đúng với mọi Scene lịch sử.

# **67. Old Scene Regeneration**

Khi regenerate Scene 10:

không đơn giản lấy current Character Reference.

Phải lấy reference/state có hiệu lực tại Scene 10 nếu domain đã định nghĩa temporal change.

# **68. Storage Security**

Asset private không nên mặc định public.

Frontend không cần biết:

storage credentials

# **69. Signed URL**

Object Storage có thể cung cấp:

short-lived signed URL

cho frontend.

Luồng:

Frontend

↓

Application API

↓

Validate ownership

↓

Generate Signed URL

↓

Frontend loads Asset

# **70. Direct Upload**

User upload Character Reference có thể dùng:

Frontend

↓

Request Upload

↓

Backend creates pending Asset

↓

Signed Upload URL

↓

Frontend uploads directly to storage

↓

Backend confirms Asset

Điều này tránh gửi file lớn qua Application API nếu không cần.

# **71. Upload Validation**

File upload cần kiểm tra:

- MIME type;

- maximum size;

- image dimensions;

- supported format;

- security scanning nếu cần.

# **72. Asset Ownership**

Mọi Asset phải có:

project_id

để kiểm tra quyền truy cập.

Không dựa vào storage folder name để xác định ownership.

# **73. Asset Reuse**

Một Asset có thể được tham chiếu nhiều nơi nếu hợp lệ.

Ví dụ một Character Reference có thể được dùng nhiều generation.

Không cần copy file mỗi lần.

# **74. Orphan Asset**

Asset là orphan nếu:

- upload thành công;

- nhưng không còn entity nào tham chiếu;

- và không có history/provenance cần giữ.

Không xóa ngay lập tức.

# **75. Orphan Cleanup**

Cleanup job có thể chạy định kỳ:

Find old unreferenced temporary assets

↓

Verify no provenance dependency

↓

Delete physical file

↓

Mark Asset DELETED

# **76. Temporary Asset**

AI Worker có thể tạo file temporary.

Temporary file:

- không phải Asset chính thức;

- không có giá trị provenance;

- cleanup sau Job.

# **77. Failed Generation Asset**

Nếu AI sinh image nhưng pipeline fail trước Candidate completion:

có thể:

1.  xóa file tạm;

2.  hoặc giữ diagnostic asset có retention ngắn.

Không để failed artifact xuất hiện như Candidate hợp lệ.

# **78. Asset Retention**

Retention policy nên phân biệt:

Canonical / Selected Assets

Generated Candidate Assets

Temporary Assets

Failed Diagnostic Assets

Archived Project Assets

# **79. MVP Retention**

Đối với thesis/MVP:

- selected output: giữ;

- valid candidates: giữ;

- references: giữ;

- temporary files: cleanup;

- failed transient files: cleanup;

- archived Project assets: giữ.

Đơn giản và an toàn hơn việc tự động xóa Candidate.

# **80. Candidate History**

Generation Candidate phải được giữ ngay cả khi:

- không selected;

- stale;

- replaced bởi Candidate mới.

Điều này phục vụ:

- generation history;

- comparison;

- research evaluation.

# **81. Candidate Archive**

Nếu quá nhiều Candidate:

có thể chuyển:

ACTIVE

→ ARCHIVED

nhưng không mất provenance.

# **82. Version Retention**

Version đã được một Context Snapshot tham chiếu không được xóa.

Ví dụ:

Candidate C

→ Snapshot S

→ CharacterVersion 3

thì CharacterVersion 3 phải giữ.

# **83. Unreferenced Old Version**

Một version chưa từng được generation dùng vẫn nên giữ ít nhất trong edit history.

Automated version garbage collection không cần cho MVP.

# **84. Snapshot Retention**

Context Snapshot nên giữ cùng lifetime với Generation Job/Candidate mà nó phục vụ.

Không xóa Snapshot trong khi Candidate vẫn tồn tại.

# **85. Snapshot Immutability**

Không được:

UPDATE context_snapshot

SET characterVersion = 4

sau khi generation bắt đầu.

Nếu cần generate mới:

tạo Snapshot mới.

# **86. Snapshot Schema Version**

Context Snapshot nên có:

schema_version

Ví dụ:

1

2

vì cấu trúc AI context có thể thay đổi trong quá trình phát triển thesis.

# **87. Why Snapshot Schema Version Matters**

Nếu vài tháng sau Context Builder thay đổi format:

ta vẫn biết Snapshot cũ phải được đọc theo schema nào.

# **88. AI Input Contract Version**

Ngoài Snapshot schema:

Generation Attempt có thể lưu:

input_contract_version

để biết worker đã nhận payload theo contract nào.

# **89. Reproducibility Levels**

Không phải mọi AI model có thể reproduce pixel-perfect.

OWNIVERSE chỉ cần phân biệt:

### **Data Reproducibility**

biết chính xác input/context/config.

### **Execution Reproducibility**

có thể chạy lại cùng environment/model.

### **Deterministic Reproducibility**

output giống hoàn toàn.

Mức cuối không phải lúc nào cũng đảm bảo được.

# **90. Research Reproducibility**

Đối với luận văn, phải giữ tối thiểu:

model identifier

model/checkpoint version

context snapshot

generation parameters

seed if applicable

software/method version

# **91. Experiment Metadata**

Generation Attempt có thể thêm:

experiment_id

method_label

Ví dụ:

BASELINE_STORYDIFFUSION

PROPOSED_METHOD_V1

Điều này rất hữu ích khi evaluation.

# **92. Baseline and Proposed Method**

Hai Candidate có thể target cùng Scene nhưng được tạo bằng phương pháp khác nhau.

Ví dụ:

Candidate A

method = BASELINE

Candidate B

method = PROPOSED

Domain Scene không cần thay đổi.

# **93. Storage Provider Abstraction**

Application sử dụng logical interface:

IAssetStorage

với các operation:

Upload

Open/Read

Delete

GenerateAccessUrl

Exists

Không để business module gọi trực tiếp S3 SDK.

# **94. Asset Storage Implementation**

Application sử dụng abstraction:

IAssetStorage

Development implementation:

LocalFileAssetStorage

Demo / production-like implementation:

S3AssetStorage

Application Domain và Generation workflow không thay đổi khi đổi storage provider.

# **95. S3-Compatible Production-Like Storage**

Demo / production-like environment sử dụng S3-compatible provider thông qua S3AssetStorage.

Cloudflare R2 là provider được ưu tiên cho thesis/demo hiện tại, nhưng provider không phải business dependency và có thể được thay bằng S3-compatible service khác thông qua configuration.

Development mặc định dùng local filesystem/volume thay vì yêu cầu một object-storage server riêng.

# **96. Storage Transaction Problem**

Database và Object Storage không nằm trong cùng database transaction.

Do đó có thể xảy ra:

File upload success

DB insert fail

hoặc:

DB row created

file upload fail

# **97. Recommended Asset Creation State**

Dùng:

PENDING

trong Asset lifecycle.

Ví dụ:

Create Asset PENDING

↓

Upload

↓

Validate

↓

Asset ACTIVE

Nếu upload fail:

FAILED

# **98. Generated Asset Flow**

Đối với Worker:

Generate Image

↓

Upload Object

↓

Create/Activate Asset Record

↓

Create Candidate

↓

Complete Job

Candidate chỉ tham chiếu Asset ACTIVE.

# **99. File Naming**

Physical filename không dùng user-provided name làm unique identifier.

Dùng:

asset_id

hoặc random object key.

Original filename chỉ là metadata.

# **100. Content-Type Trust**

Không chỉ tin file extension:

image.png

System nên xác minh MIME/actual content khi có thể.

# **101. Duplicate Assets**

Không bắt buộc deduplicate file vật lý trong MVP.

Checksum được lưu chủ yếu cho integrity.

Content-addressable storage có thể thêm sau.

# **102. Deletion Semantics**

Có ba cấp độ:

Archive Domain Reference

Delete Asset Metadata

Delete Physical Object

Không phải lúc nào ba hành động cũng xảy ra cùng lúc.

# **103. Character Reference Removal**

Nếu user bỏ Character Reference khỏi active use:

CharacterReference → ARCHIVED

Asset vẫn có thể được giữ vì Generation cũ đã sử dụng nó.

# **104. Project Deletion**

Nếu user thật sự yêu cầu permanent deletion trong future production:

cần cascade-aware deletion workflow riêng.

Không dùng:

DELETE project CASCADE

mù quáng.

# **105. History Preservation**

Nếu Project chỉ ARCHIVED:

không xóa:

- versions;

- snapshots;

- candidates;

- assets;

- jobs.

# **106. Candidate Dependency Model**

Ngoài Snapshot dependency, có thể cần query nhanh:

Which candidates depend on Character A?

Ta có thể đi:

Candidate

→ Job

→ Snapshot

→ Dependencies

Không cần duplicate CandidateDependency table trong MVP.

# **107. Dependency Query Example**

Character A changed

↓

Find ContextSnapshotDependencies

where entity_id = A

↓

Find Snapshots

↓

Find Jobs

↓

Find Candidates

# **108. Indexes**

Các index quan trọng:

assets(project_id)

assets(status)

character_versions(character_id, version_number)

scene_versions(scene_id, version_number)

story_core_versions(story_core_id, version_number)

context_snapshots(story_id)

context_snapshots(target_type, target_id)

context_snapshot_dependencies(entity_type, entity_id)

context_snapshot_dependencies(context_snapshot_id)

# **109. Unique Constraints**

Recommended:

UNIQUE(character_id, version_number)

UNIQUE(scene_id, version_number)

UNIQUE(story_core_id, version_number)

# **110. Snapshot Dependency Constraint**

Có thể dùng:

UNIQUE(

context_snapshot_id,

entity_type,

entity_id,

version_number,

dependency_role

)

để tránh duplicate dependency record.

# **111. Version Transaction**

Canonical Character update:

BEGIN

Load Character

Validate expected revision

Insert CharacterVersion N+1

Update characters current state

Increment revision

COMMIT

Nếu insert version thất bại:

canonical current state cũng không được đổi.

# **112. Snapshot Creation Transaction**

Trước generation:

BEGIN

Resolve required entity versions

Create ContextSnapshot

Create SnapshotDependencies

Create GenerationJob

COMMIT

Sau đó mới publish Queue message.

# **113. Snapshot and Job Consistency**

GenerationJob không được trỏ tới Snapshot không tồn tại.

Foreign key:

generation_jobs.context_snapshot_id

→ context_snapshots.context_snapshot_id

# **114. Snapshot Creation Timing**

Snapshot phải được tạo:

> **sau khi request được validate nhưng trước khi Job được worker thực thi.**

# **115. Version Race Condition**

Ví dụ:

User bấm Generate.

Trong cùng lúc Character đang bị edit.

Backend phải đảm bảo Snapshot lấy một state nhất quán.

Có thể sử dụng short database transaction hoặc concurrency revision validation.

# **116. Snapshot Consistency**

Không chấp nhận Snapshot kiểu:

StoryCore before update

Character after update

Scene half-updated

nếu chúng thuộc cùng canonical transaction logic.

Snapshot creation cần đọc một consistent database view phù hợp.

# **117. Large Snapshot Data**

Không nên copy mọi Story content vào snapshot_data_json nếu không cần.

Snapshot nên chứa:

- normalized context payload cần AI;

- references tới versions;

- relevant facts;

- generation-specific derived context.

# **118. Snapshot vs Version**

Hai khái niệm không được nhầm:

**Version**

lịch sử của một entity.

**Snapshot**

bộ kết hợp nhiều entity/context dùng cho một Job.

Ví dụ:

CharacterVersion 3

SceneVersion 5

StoryCoreVersion 7

↓

ContextSnapshot 102

# **119. Snapshot Reuse**

Không nên mặc định reuse Snapshot giữa nhiều Job.

Ngay cả khi context giống nhau, tạo Snapshot riêng giúp provenance dễ hiểu hơn.

Optimization có thể thêm sau.

# **120. Candidate Selection History**

Scene hiện tại chỉ cần:

selected_candidate_id

Nếu muốn biết user từng chọn Candidate nào trước đó:

có thể dùng:

candidate_selection_history

nhưng không bắt buộc MVP.

Version của Scene cũng có thể ghi selection thay đổi.

# **121. Storage Lifecycle**

Có thể hình dung:

CREATED

↓

PENDING

↓

ACTIVE

↓

ARCHIVED

↓

DELETED

Không phải Asset nào cũng đi qua mọi state.

# **122. Storage Failure**

Nếu Asset Storage unavailable:

Generation Job không được COMPLETED.

Job có thể:

RETRY_PENDING

nếu lỗi transient.

# **123. Storage Observability**

Nên theo dõi:

- upload latency;

- upload failures;

- storage capacity;

- orphan assets;

- failed cleanup;

- signed URL errors.

# **124. Backup Strategy**

Database backup và Object Storage backup là hai việc riêng.

Nếu chỉ backup database mà mất object storage:

Candidate image mất.

Nếu chỉ backup asset mà mất database:

không biết file thuộc Story nào.

# **125. Backup Consistency**

Production-like environment nên có:

Database Backup

\+

Object Storage Durability

MVP thesis có thể đơn giản hơn nhưng vẫn cần hiểu dependency này.

# **126. Export**

Nếu user export Project:

hệ thống có thể package:

Story Data

Selected Images

Optional Candidate History

Export file bản thân nó cũng có thể được lưu như Asset.

# **127. Storage Privacy**

Asset của Project private không được accessible chỉ vì biết asset_id.

Backend phải xác nhận Project ownership.

# **128. CDN Boundary**

CDN có thể được thêm để serve image nhanh.

Không ảnh hưởng domain model.

Object Storage vẫn là persistent source.

# **129. DATA-02 Logical Tables**

Sau DATA-01, DATA-02 bổ sung chủ yếu:

assets

asset_variants

character_versions

scene_versions

story_core_versions

context_snapshots

context_snapshot_dependencies

Một số bảng khác là optional enhancement.

# **130. Simplified Relationship**

CHARACTER

↓

CHARACTER_VERSION

\\

\\

STORY_CORE_VERSION ──→ CONTEXT_SNAPSHOT

/ ↓

SCENE_VERSION JOB

↓

ATTEMPT

↓

CANDIDATE

↓

ASSET

Giải thích bằng lời:

Canonical entities có nhiều Version. Khi user yêu cầu generation, hệ thống chọn đúng các Version cần thiết và đóng chúng vào Context Snapshot. Job sử dụng Snapshot đó. AI tạo Candidate và Candidate tham chiếu file thật trong Asset Storage.

# **131. Long-Range Example**

Giả sử:

Scene 1:

A has long black hair.

Scene 20:

A cuts hair short.

Scene 40:

Current Character = short hair.

User regenerate:

Scene 5

System tìm:

Character version effective at Scene 5

ví dụ:

CharacterVersion 3

rồi tạo:

ContextSnapshot S501

Snapshot chứa:

Character A v3

Scene 5 v7

StoryCore v12

Generation dùng Snapshot đó.

Không dùng current Character v9 ở Scene 40.

# **132. Candidate Example**

Sau generation:

Job J500

↓

Attempt A1

↓

Candidate C1

↓

Asset IMG-100

Candidate C1 truy ngược:

C1

→ J500

→ S501

→ Character A v3

Ta có thể chứng minh chính xác ảnh được tạo trong context nào.

# **133. Character Change Example**

Sau đó user chỉnh:

Character A v10

System tìm Candidate từng dùng A v3.

C1 có thể được:

POTENTIALLY_STALE

nhưng:

- không xóa;

- không overwrite;

- không tự regenerate.

# **134. Requirements**

### **DATA2-FR-01**

Generated binary files SHALL được lưu ngoài relational database core.

### **DATA2-FR-02**

Asset metadata SHALL được persistence trong database.

### **DATA2-FR-03**

Generated Asset SHALL không bị overwrite khi regeneration.

### **DATA2-FR-04**

Canonical Character, Scene và Story Core SHALL hỗ trợ historical version records.

### **DATA2-FR-05**

Version records SHALL immutable sau khi tạo.

### **DATA2-FR-06**

Rollback SHALL tạo version mới thay vì chỉnh sửa lịch sử.

### **DATA2-FR-07**

Generation SHALL tham chiếu immutable Context Snapshot.

### **DATA2-FR-08**

Context Snapshot SHALL lưu hoặc tham chiếu các entity versions được sử dụng.

### **DATA2-FR-09**

System SHALL hỗ trợ query dependencies theo entity/version.

### **DATA2-FR-10**

Candidate SHALL truy ngược được tới Job, Attempt và Context Snapshot.

### **DATA2-FR-11**

Candidate becoming stale SHALL không tự động xóa Candidate.

### **DATA2-FR-12**

Character Reference history SHALL được giữ khi đã được generation sử dụng.

### **DATA2-FR-13**

Private Asset SHALL yêu cầu authorization trước khi access.

### **DATA2-FR-14**

Temporary files SHALL có cleanup policy.

### **DATA2-FR-15**

Research generation SHALL lưu model/config metadata cần thiết cho reproducibility.

# **135. Non-Functional Requirements**

### **DATA2-NFR-01 — Traceability**

Mỗi generated output phải truy được nguồn context.

### **DATA2-NFR-02 — Durability**

Selected/reference assets phải có persistent storage.

### **DATA2-NFR-03 — Portability**

Storage abstraction không phụ thuộc một provider duy nhất.

### **DATA2-NFR-04 — Reproducibility**

Research metadata không được mất khi Candidate archived.

### **DATA2-NFR-05 — Security**

Storage credential không được expose ra frontend.

### **DATA2-NFR-06 — Scalability**

Số lượng generated images tăng không được làm relational database phình vì binary blobs.

# **136. Acceptance Criteria**

### **AC-DATA2-01**

Given Candidate X được tạo bằng Character v3,  
when Character hiện tại trở thành v5,  
then system vẫn xác định được Candidate X dùng v3.

### **AC-DATA2-02**

Given user regenerate một Scene,  
then hệ thống phải tạo Candidate và Asset mới thay vì overwrite output cũ.

### **AC-DATA2-03**

Given Character Reference bị thay đổi,  
then reference cũ đã dùng bởi Generation trước đó phải vẫn truy vết được.

### **AC-DATA2-04**

Given Context Snapshot đã được Generation Job sử dụng,  
then Snapshot không được thay đổi.

### **AC-DATA2-05**

Given generated image upload thất bại,  
then Candidate image không được đánh dấu hoàn chỉnh.

### **AC-DATA2-06**

Given Candidate được archive,  
then provenance của Candidate vẫn phải tồn tại.

### **AC-DATA2-07**

Given user restore Character từ version cũ,  
then system phải tạo version mới đại diện cho restore.

### **AC-DATA2-08**

Given Project asset private,  
when user không có quyền Project,  
then system không được cấp access URL.

### **AC-DATA2-09**

Given user regenerate historical Scene,  
then Context Snapshot phải có khả năng tham chiếu historical Character state/version phù hợp.

### **AC-DATA2-10**

Given cùng một Scene được generate bằng baseline và proposed method,  
then hệ thống phải giữ được model/method metadata riêng cho từng Attempt.

# **137. Traceability**

Ví dụ:

PROD:

Long-Range Character Consistency

↓

DOMAIN-02:

Historical State

Version

↓

DATA-02:

CharacterVersion

ContextSnapshot

SnapshotDependency

↓

AI-02:

Resolve Scene-effective Context

↓

AI-03:

Generate / Validate

↓

QA-02:

Compare Consistency

# **138. Key Architecture Decisions**

**ADR-DATA2-01  
**Binary assets lưu ngoài PostgreSQL.

**ADR-DATA2-02  
**Generated assets immutable.

**ADR-DATA2-03  
**Version history sử dụng full snapshots trong MVP.

**ADR-DATA2-04  
**StoryCore, Character và Scene là các entity versioning ưu tiên.

**ADR-DATA2-05  
**Context Snapshot immutable.

**ADR-DATA2-06  
**Snapshot Dependencies được lưu relational để hỗ trợ stale detection.

**ADR-DATA2-07  
**Restore tạo version mới.

**ADR-DATA2-08  
**Stale Candidate không bị xóa hoặc tự regenerate.

**ADR-DATA2-09  
**Character References đã được dùng phải giữ historical trace.

**ADR-DATA2-10  
**Storage được truy cập thông qua abstraction, không phụ thuộc provider.

# **139. MVP Scope**

Để tránh database lại phình thành quá nhiều table, **không cần implement toàn bộ DATA-02 ngay**.

MVP thực tế chỉ cần bổ sung bốn phần chính vào DATA-01:

assets

character_versions

scene_versions

story_core_versions

context_snapshots

context_snapshot_dependencies

Tức khoảng **6 bảng cốt lõi**.

asset_variants, stale evaluation history hay selection history có thể bổ sung sau.

# **140. Recommended MVP Storage Flow**

Canonical Character / Scene / Story Core

↓

Version

↓

Generate requested

↓

Context Snapshot

↓

AI Job

↓

Candidate

↓

Asset

Đây là flow quan trọng nhất cần triển khai.

# **141. Boundary With DATA-01**

Có thể nhớ rất đơn giản:

**DATA-01**

> Story hiện tại được lưu như thế nào?

Ví dụ:

characters

scenes

story_facts

**DATA-02**

> Story trước đây như thế nào, AI đã nhìn thấy gì và output được lưu ở đâu?

Ví dụ:

character_versions

context_snapshots

assets

# **142. Data Group Final Model**

Sau DATA-01 và DATA-02, toàn bộ persistence của OWNIVERSE có thể nhìn thành:

OWNIVERSE DATA

PROJECT

↓

STORY

↓

CANONICAL STORY DATA

┌───────────┼───────────┐

↓ ↓ ↓

Story Core Character Scene

│ │ │

└───────────┼───────────┘

↓

Versions

↓

Context Snapshot

↓

Generation Job

↓

Attempt

↓

Candidate

↓

Asset

Giải thích bằng lời:

Ở tầng trên là câu chuyện chính thức mà creator sở hữu. Khi dữ liệu thay đổi, hệ thống giữ Version. Khi AI cần generate, hệ thống chọn đúng các version và đóng chúng thành Context Snapshot. Job chạy dựa trên Snapshot, tạo Candidate, còn file thật được lưu thành Asset. Không phần nào trong generation được phép quay ngược lại ghi đè Canon một cách tự động.

# **143. Core Principle**

Nguyên tắc cuối cùng của DATA-02:

> **OWNIVERSE must remember not only what it generated, but why that generation looked the way it did.**

Nói đơn giản:

Không chỉ cần lưu **“đây là bức ảnh AI đã tạo”**.

Ta còn phải biết:

> **AI tạo ảnh đó cho Scene nào, dùng Character ở version nào, dùng Story Core nào, reference nào, model nào và configuration nào.**

Đây chính là nền dữ liệu để sau này chúng ta làm **long-range consistency, stale detection, generation history và evaluation cho luận văn** một cách nghiêm túc.
