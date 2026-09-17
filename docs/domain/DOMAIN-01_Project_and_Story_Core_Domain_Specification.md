# **DOMAIN-01 — Project & Story Core Domain Specification**

**Document ID:** DOMAIN-01  
**Document Type:** Domain Specification  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Primary Role:** Domain Architect / Business Analyst  
**Status:** Draft v1  
**Parent Documents:** PROD-01, PROD-02, ARCH-01  
**Related Documents:** DOMAIN-02, DATA-01, API-01, AI-02, AI-03

## **1. Purpose**

Tài liệu này định nghĩa **mô hình nghiệp vụ cốt lõi** của OWNIVERSE.

Mục tiêu là xác định chính xác:

- một Project chứa những gì;

- Story Core là gì;

- Character được mô hình hóa như thế nào;

- Scene liên hệ với Character ra sao;

- đâu là dữ liệu chính thức của câu chuyện;

- đâu chỉ là dữ liệu AI đề xuất hoặc sinh ra;

- Story, Character, Scene, World và generated output liên hệ nhau như thế nào;

- phần nào của domain sẽ trở thành nguồn dữ liệu cho AI context.

Tài liệu này mô tả **ý nghĩa của dữ liệu**, không mô tả database table cụ thể.

# **2. Domain Principle**

Nguyên tắc trung tâm của OWNIVERSE:

> **A story is not a collection of AI generations. It is a structured creative world owned by the creator.**

AI có thể:

- đề xuất;

- mở rộng;

- generate;

- phân tích;

- kiểm tra.

Nhưng **AI output không tự động trở thành sự thật của câu chuyện**.

# **3. Domain Boundary**

Domain chính của OWNIVERSE gồm:

User

↓

Project

↓

Story

↓

Story Core

├── Story Identity

├── World

├── Characters

├── Relationships

├── Story Facts

└── Creative Direction

Story

↓

Scenes

↓

Scene Characters

↓

Generated Candidates

Giải thích bằng lời:

Project là workspace lớn nhất. Bên trong Project có Story. Story Core chứa những thông tin cốt lõi giúp xác định “câu chuyện này là gì”. Story sau đó được triển khai thành các Scene. Scene sử dụng Character và cuối cùng có thể tạo ra các generated outputs.

# **4. Core Domain Entities**

Các domain object chính:

- Project

- Story

- Story Core

- World

- Character

- Character Reference

- Character Relationship

- Story Fact

- Scene

- Scene Character

- Scene State

- Generation Candidate

- Selected Output

- Asset

DOMAIN-01 định nghĩa ý nghĩa và quan hệ của chúng.

# **5. Project**

## **5.1 Definition**

**Project** là creative workspace cấp cao nhất của người dùng.

Project tập hợp toàn bộ dữ liệu cần thiết để phát triển một tác phẩm.

## **5.2 Project Responsibilities**

Project quản lý:

- Story;

- Characters;

- Scenes;

- Assets;

- Generation History;

- project metadata;

- ownership;

- lifecycle.

## **5.3 Project Core Fields**

Ở mức domain, Project tối thiểu cần:

Project

\- ProjectId

\- OwnerId

\- Title

\- Description

\- Status

\- CoverAsset

\- CreatedAt

\- UpdatedAt

Đây là logical model, không phải database schema.

# **6. Project Lifecycle**

Project có thể có các trạng thái:

DRAFT

ACTIVE

COMPLETED

ARCHIVED

### **DRAFT**

Project mới được tạo và chưa đủ dữ liệu.

### **ACTIVE**

Project đang được phát triển.

### **COMPLETED**

Creator xem story hiện tại là hoàn thiện.

### **ARCHIVED**

Project không còn active nhưng vẫn được lưu.

# **7. Project Invariant**

## **DOM-INV-01**

Mỗi Project phải thuộc về một Owner.

## **DOM-INV-02**

Mọi Story, Scene, Character và Asset phải truy ngược được về Project.

## **DOM-INV-03**

Project bị archive không đồng nghĩa với việc xóa dữ liệu.

# **8. Story**

## **8.1 Definition**

**Story** là tác phẩm kể chuyện chính bên trong Project.

Trong MVP:

> Một Project có một Story chính.

Kiến trúc có thể mở rộng để một Project chứa nhiều Story trong tương lai, nhưng không cần thiết ở MVP.

# **9. Story Responsibilities**

Story chịu trách nhiệm tập hợp:

- Story Core;

- Characters;

- Scenes;

- continuity information;

- creative direction;

- generated outputs liên quan.

# **10. Story Core**

## **10.1 Definition**

**Story Core** là nguồn thông tin canonical ở cấp toàn bộ câu chuyện.

Nó không phải một đoạn prompt lớn.

Nó là một tập dữ liệu có cấu trúc mô tả:

> câu chuyện này nói về điều gì, diễn ra trong thế giới nào, có những luật gì và định hướng sáng tạo ra sao.

# **11. Why Story Core Exists**

Nếu chỉ lưu một đoạn description như:

> “Một cô gái đi khám phá thế giới phép thuật...”

thì hệ thống không đủ khả năng biết:

- nhân vật nào là nhân vật chính;

- thế giới có luật gì;

- yếu tố nào không được thay đổi;

- tone là gì;

- mục tiêu của câu chuyện là gì;

- đâu là sự thật đã được creator chấp nhận.

Story Core giải quyết vấn đề đó.

# **12. Story Core Structure**

Story Core được chia thành các nhóm:

Story Core

├── Story Identity

├── Narrative Foundation

├── World Foundation

├── Creative Direction

├── Story Facts

└── Global Constraints

# **13. Story Identity**

Story Identity chứa các thông tin định danh cơ bản:

- title;

- short description;

- genre;

- target audience;

- language;

- story format.

Ví dụ format:

- manga;

- illustrated story;

- comic;

- storyboard;

- visual novel style.

OWNIVERSE không khóa domain vào Manga.

# **14. Narrative Foundation**

Narrative Foundation mô tả cốt lõi của câu chuyện.

Có thể gồm:

- premise;

- central conflict;

- main theme;

- protagonist goal;

- stakes;

- narrative tone;

- ending direction nếu creator xác định.

# **15. Premise**

Premise trả lời:

> “Câu chuyện cơ bản này nói về điều gì?”

Ví dụ:

> Một cô gái mất trí nhớ đi qua nhiều thành phố để tìm lại quá khứ, nhưng dần phát hiện ký ức của mình liên quan đến sự sụp đổ của thế giới.

Premise không phải toàn bộ synopsis.

# **16. Synopsis**

Synopsis mô tả nội dung ở mức rộng hơn.

Nó có thể được creator:

- viết thủ công;

- generate bằng AI;

- sửa lại;

- xác nhận.

AI-generated synopsis chưa được xác nhận không tự động trở thành canonical.

# **17. Theme**

Theme là ý tưởng lớn mà Story muốn truyền tải.

Ví dụ:

- identity;

- friendship;

- sacrifice;

- loneliness;

- growing up;

- freedom.

Theme có thể hỗ trợ AI nhưng không phải command cứng.

# **18. Creative Direction**

Creative Direction chứa định hướng sáng tạo chung.

Ví dụ:

- visual style;

- narrative tone;

- pacing preference;

- emotional direction;

- artistic references;

- storytelling style.

Ví dụ:

Visual Style:

cinematic anime fantasy

Tone:

melancholic but hopeful

# **19. Creative Direction vs AI Prompt**

Creative Direction không phải prompt cuối cùng.

Luồng đúng là:

Creative Direction

↓

AI Context Builder

↓

Generation Instruction

↓

Model Prompt

AI layer quyết định cách chuyển domain data thành model input.

# **20. World**

## **20.1 Definition**

**World** mô tả bối cảnh và các quy tắc của thế giới truyện.

World có thể đơn giản hoặc phức tạp tùy Story.

# **21. World Structure**

World có thể gồm:

- name;

- description;

- time period;

- technology level;

- magic rules;

- social structure;

- important locations;

- factions;

- global laws;

- environmental characteristics.

Không phải Story nào cũng cần đầy đủ tất cả trường này.

# **22. World Rule**

World Rule là một quy tắc creator xác định là đúng.

Ví dụ:

> Humans cannot use magic directly.

hoặc:

> The city is permanently covered in artificial night.

World Rule có thể trở thành canonical Story Fact.

# **23. Location**

Location là một địa điểm có ý nghĩa trong Story.

Ví dụ:

Location

\- Name

\- Description

\- Environment

\- Visual Traits

\- Related Facts

Location có thể được Scene tham chiếu.

# **24. Character**

## **24.1 Definition**

**Character** là một thực thể nhân vật có identity độc lập trong Story.

Character được nhận diện bằng internal Character ID.

Không dựa vào tên.

# **25. Character Identity Rule**

## **DOM-CHAR-01**

Hai nhân vật có cùng tên vẫn là hai Character khác nhau nếu Character ID khác nhau.

## **DOM-CHAR-02**

Một nhân vật đổi tên vẫn là cùng Character nếu Character ID không đổi.

Điều này đặc biệt quan trọng cho AI consistency.

# **26. Character Structure**

Character được chia thành:

Character

├── Profile

├── Stable Identity

├── Base Appearance

├── Personality

├── Narrative Role

├── Relationships

├── References

└── Locks

# **27. Character Profile**

Profile có thể gồm:

- name;

- aliases;

- age;

- gender nếu có;

- role;

- short biography;

- description.

Profile phục vụ creator và story.

# **28. Stable Identity**

Stable Identity chứa các đặc điểm giúp định nghĩa:

> “đây là nhân vật nào”.

Ví dụ:

- facial identity;

- distinctive physical traits;

- body characteristics;

- identity-defining attributes.

Đây là phần quan trọng nhất đối với character consistency.

# **29. Stable Identity vs Appearance**

Hai khái niệm phải được tách rõ.

Ví dụ:

Stable Identity:

blue eyes

short black hair

scar under left eye

Scene Appearance:

winter coat

wet hair

angry expression

Stable Identity thường không thay đổi tùy Scene.

Appearance có thể thay đổi.

# **30. Base Appearance**

Base Appearance là diện mạo mặc định của Character.

Ví dụ:

- default hairstyle;

- normal outfit;

- accessories;

- color palette;

- common visual presentation.

Nó không có nghĩa Character luôn phải mặc đúng outfit đó.

# **31. Character Locks**

Creator có thể khóa một số thuộc tính.

Ví dụ:

Eye Color → LOCKED

Hair Color → LOCKED

Scar → LOCKED

Default Outfit → UNLOCKED

AI không được tự động thay đổi locked canonical attributes.

# **32. Character Reference**

Character Reference là visual reference liên kết với Character.

Ví dụ:

- uploaded reference;

- generated approved portrait;

- canonical reference image.

Một Character có thể có nhiều references.

# **33. Primary Reference**

Một reference có thể được đánh dấu:

Current Reference

hoặc primary reference.

Nó đại diện cho visual identity hiện tại được creator ưu tiên.

# **34. Character Reference Rule**

Generated image không tự động trở thành Character Reference.

Creator hoặc workflow phải xác nhận.

# **35. Character Relationship**

Character Relationship mô tả mối quan hệ giữa hai Character.

Ví dụ:

- friend;

- sibling;

- rival;

- enemy;

- mentor;

- romantic interest.

# **36. Relationship Direction**

Một relationship có thể directional.

Ví dụ:

A trusts B

B distrusts A

không giống:

A and B are siblings

Domain phải cho phép biểu đạt cả hai dạng.

# **37. Character Role**

Character Role có thể gồm:

- protagonist;

- antagonist;

- supporting;

- recurring;

- minor.

Đây là narrative role, không phải authorization role.

# **38. Scene**

## **38.1 Definition**

**Scene** là đơn vị kể chuyện cơ bản mà OWNIVERSE dùng cho workflow sáng tạo và image generation.

Scene mô tả một khoảnh khắc hoặc đoạn sự kiện có ý nghĩa.

# **39. Why Scene is Core Unit**

Scene phù hợp vì nó là nơi hội tụ:

- story context;

- characters;

- location;

- action;

- emotion;

- visual generation.

Do đó Scene là boundary tự nhiên giữa product và AI generation.

# **40. Scene Structure**

Scene gồm:

Scene

├── Scene Identity

├── Narrative Content

├── Location

├── Active Characters

├── Character States

├── Scene Facts

├── Creative Direction

└── Selected Output

# **41. Scene Identity**

Tối thiểu:

- SceneId;

- title;

- order/index;

- summary;

- status.

# **42. Scene Narrative Content**

Có thể chứa:

- description;

- action;

- dialogue summary;

- emotional beat;

- narrative purpose.

Không bắt buộc Scene phải chứa screenplay hoàn chỉnh.

# **43. Scene Ordering**

Scene có explicit order.

Ví dụ:

Scene 1

Scene 2

Scene 3

...

Không dựa vào creation time để suy ra thứ tự truyện.

# **44. Chapter**

Chapter là optional organizational layer.

Nếu sử dụng:

Story

↓

Chapter

↓

Scene

Nếu Story ngắn:

Story

↓

Scene

MVP không bắt buộc Chapter.

# **45. Scene Character**

Character xuất hiện trong Scene thông qua relation:

Scene

↓

SceneCharacter

↓

Character

SceneCharacter không chỉ là join relation.

Nó còn chứa trạng thái của Character trong Scene.

# **46. Scene Character State**

Có thể bao gồm:

- outfit;

- expression;

- pose/action;

- physical condition;

- carried items;

- local appearance note;

- scene role.

# **47. Example**

Character canonical:

Character A

Hair: Black

Eyes: Blue

Scar: Left cheek

Scene 7:

Outfit: White winter jacket

Expression: Exhausted

Condition: Wet from rain

Holding: Broken umbrella

Thông tin Scene 7 không được overwrite Character canonical identity.

# **48. Scene Character Rule**

## **DOM-SC-01**

Scene-specific appearance SHALL NOT tự động thay đổi Character canonical appearance.

## **DOM-SC-02**

Mỗi SceneCharacter phải tham chiếu một Character tồn tại trong cùng Story/Project.

# **49. Active Character**

Active Character là Character thực sự tham gia Scene.

AI context mặc định chỉ cần Character liên quan đến Scene.

Ví dụ:

Scene có:

Character A

Character B

thì không nên mặc định đưa:

Character C

Character D

Character E

vào generation context.

Điều này hỗ trợ trực tiếp AI-02.

# **50. Story Fact**

## **50.1 Definition**

**Story Fact** là một sự thật được Story chấp nhận là đúng.

Ví dụ:

> A lost her mother when she was ten.
>
> The northern gate was destroyed.
>
> B knows A's real identity.

Story Facts hỗ trợ continuity.

# **51. Story Fact Scope**

Fact có thể thuộc phạm vi:

- Story;

- World;

- Character;

- Relationship;

- Scene.

# **52. Fact Status**

Ở mức domain có thể phân biệt:

PROPOSED

CANONICAL

RETIRED

Chi tiết lịch sử thay đổi được xử lý sâu hơn trong DOMAIN-02.

# **53. Canonical Data**

Canonical Data là dữ liệu hệ thống hiện xem là sự thật chính thức của Story.

Ví dụ:

- approved Story Core;

- Character identity;

- confirmed World Rule;

- accepted Story Fact;

- Scene data creator đã lưu.

# **54. Canonical vs Generated**

Điểm cực kỳ quan trọng:

AI Generated

≠

Canonical

Ví dụ AI đề xuất:

> Character A has a younger brother.

Nếu creator chưa chấp nhận:

đây chỉ là proposal.

# **55. AI Proposal**

AI Proposal là dữ liệu do AI đề xuất nhằm thay đổi hoặc mở rộng domain.

Proposal phải có trạng thái riêng.

Ví dụ:

AI Proposal

\- target

\- proposed change

\- source job

\- status

# **56. Proposal States**

Có thể gồm:

PENDING

ACCEPTED

REJECTED

SUPERSEDED

Chỉ ACCEPTED mới có thể dẫn tới canonical update.

# **57. Generated Candidate**

Generated Candidate là một output AI cụ thể.

Ví dụ:

- image;

- scene text;

- synopsis option;

- character design.

Candidate không phải Story Fact.

# **58. Candidate Structure**

Logical model:

Candidate

\- CandidateId

\- JobId

\- Target

\- Asset / Content

\- Validation Result

\- Status

Generation details nằm sâu hơn trong AI-03 và DATA documents.

# **59. Selected Output**

Selected Output là Candidate được user chọn cho một target.

Ví dụ:

Scene 7

SelectedCandidateId = Candidate B

# **60. Selection Rule**

Selecting a Candidate:

does not mutate the Candidate itself

Scene chỉ tham chiếu Candidate được chọn.

# **61. Selection vs Canon**

Việc chọn generated image không có nghĩa tất cả chi tiết AI vô tình vẽ ra trở thành canonical Story Facts.

Ví dụ:

AI vẽ thêm chiếc vòng tay mà Story không hề định nghĩa.

Việc chọn ảnh đó không tự động tạo:

Character wears magical bracelet

thành canon.

# **62. Asset**

Asset là file hoặc resource vật lý.

Ví dụ:

- generated image;

- uploaded reference image;

- cover;

- thumbnail.

Asset không mang toàn bộ ý nghĩa domain.

Ý nghĩa đến từ entity tham chiếu Asset.

# **63. Story Core Ownership**

Story Core thuộc Story.

Không thuộc AI model.

Không thuộc Scene.

Không thuộc individual generation job.

# **64. Story Core Update Rule**

Mọi thay đổi Story Core phải qua application command hợp lệ.

AI không update trực tiếp.

# **65. Source of Truth Hierarchy**

Nếu có xung đột dữ liệu:

Explicit Canonical Data

↓

Locked Character / World Attributes

↓

Current Scene Data

↓

Accepted AI Proposal

↓

Generated Candidate

↓

AI Assumption

Dữ liệu ở trên có độ ưu tiên cao hơn dữ liệu bên dưới.

# **66. Character Source of Truth**

Ví dụ Character có canonical:

Eye Color = Blue

AI-generated image tạo:

Eye Color = Green

Không được suy ra canonical Character chuyển sang mắt xanh lá.

Đây là generation inconsistency.

# **67. Domain Consistency Rules**

## **DOM-RULE-01**

Canonical Story data SHALL take precedence over generated assumptions.

## **DOM-RULE-02**

Locked attributes SHALL NOT be silently overwritten.

## **DOM-RULE-03**

Scene-local state SHALL NOT automatically mutate Character global state.

## **DOM-RULE-04**

Generated output SHALL NOT become canonical without explicit workflow.

## **DOM-RULE-05**

Character identity SHALL be determined by Character ID.

## **DOM-RULE-06**

Scene ordering SHALL be explicit.

## **DOM-RULE-07**

Generated Candidate SHALL retain its source Job reference.

# **68. Story Structure**

Recommended MVP hierarchy:

Project

└── Story

├── Story Core

├── Characters

├── Relationships

├── World

├── Story Facts

├── Scenes

│ └── Scene Characters

└── Generated Outputs

# **69. Project Structure**

Ở mức application organization:

Project

├── Overview

├── Story

│ └── Story Core

├── Characters

├── Scenes

├── Assets

├── Generation History

└── Settings

Đây là domain grouping, không nhất thiết tương ứng 1:1 với folder hoặc database table.

# **70. Story Core Suggested Sections**

Story Core UI/domain nên hỗ trợ các section:

### **Core Idea**

- premise;

- synopsis;

- theme.

### **Narrative**

- protagonist goal;

- conflict;

- stakes;

- tone.

### **World**

- setting;

- locations;

- world rules.

### **Creative Direction**

- storytelling style;

- visual direction;

- pacing.

### **Canon**

- global facts;

- locked decisions.

# **71. Required vs Optional Data**

Không nên bắt user điền toàn bộ Story Core trước khi tạo Scene.

Required MVP:

- Project title;

- basic Story idea.

Các dữ liệu khác có thể được bổ sung dần.

# **72. Progressive Story Building**

Domain phải hỗ trợ:

Simple Idea

↓

Partial Story Core

↓

Characters

↓

Scenes

↓

Richer Story Core

Không yêu cầu Story hoàn chỉnh trước khi user bắt đầu sáng tạo.

# **73. Incomplete Domain State**

Entity có thể tồn tại trong trạng thái incomplete.

Ví dụ Character mới chỉ có:

Name

Description

vẫn hợp lệ.

System có thể cảnh báo thiếu reference nhưng không nhất thiết block toàn bộ workflow.

# **74. Validation Levels**

Domain validation có thể chia:

### **Required**

Thiếu thì operation không thể tiếp tục.

### **Recommended**

Thiếu nhưng vẫn có thể tiếp tục.

### **Optional**

Chỉ tăng chất lượng.

Ví dụ generate image có thể yêu cầu Scene description nhưng Character Reference chỉ là recommended.

# **75. Domain and AI Context**

DOMAIN-01 xác định **dữ liệu tồn tại**.

AI-02 xác định:

> trong số dữ liệu đó, AI cần lấy gì cho một task cụ thể.

Ví dụ DOMAIN-01 có:

30 Characters

100 Scenes

50 Story Facts

không có nghĩa mỗi generation đều nhận toàn bộ chúng.

# **76. Context Eligibility**

Các domain object có thể được xem là nguồn AI context:

- Story Core;

- relevant World facts;

- active Characters;

- Character References;

- Scene Character states;

- current Scene;

- relevant prior continuity;

- Creative Direction.

Retrieval logic thuộc AI-02.

# **77. Domain and Research Boundary**

Research model có thể sử dụng:

- Character identity;

- previous appearance;

- scene position;

- reference images.

Nhưng DOMAIN-01 không định nghĩa:

- embedding;

- memory vector;

- attention mechanism;

- model architecture.

Những phần đó không phải product domain.

# **78. Domain and Database Boundary**

DOMAIN-01 nói:

> Character có Stable Identity và References.

DATA-01 sau này mới quyết định:

- table nào;

- foreign key nào;

- JSON hay relational;

- index nào.

# **79. Domain and API Boundary**

DOMAIN-01 định nghĩa:

Character

Scene

Story Fact

API-01 sau này định nghĩa:

POST /...

GET /...

PATCH /...

Không trộn endpoint vào domain specification.

# **80. Domain Aggregate Boundaries**

Ở mức conceptual, các aggregate chính:

### **Project Aggregate**

Project metadata và ownership.

### **Story Aggregate**

Story Core và story-level canonical information.

### **Character Aggregate**

Character identity, appearance và references.

### **Scene Aggregate**

Scene content và SceneCharacter states.

### **Generation Aggregate**

Jobs, attempts và candidates.

Exact transaction boundaries sẽ được điều chỉnh trong implementation design.

# **81. Character–Scene Relationship**

Logical relationship:

Character

↑

SceneCharacter

↓

Scene

Đây là many-to-many.

Một Character xuất hiện nhiều Scene.

Một Scene chứa nhiều Character.

# **82. Location–Scene Relationship**

Một Scene có thể tham chiếu:

Primary Location

và optional supporting location/context.

Không cần copy toàn bộ Location description vào Scene.

# **83. Story Fact Relationships**

Story Fact có thể tham chiếu:

Subjects

Targets

Source Scene

Ví dụ:

> A discovers B is her brother.

Subjects:

A

B

Source:

Scene 18

Chi tiết temporal canon sẽ nằm ở DOMAIN-02.

# **84. Deletion Semantics**

Domain entity quan trọng không nên hard-delete tùy tiện nếu đã được generation tham chiếu.

Ví dụ:

Character đã xuất hiện trong Scene và Candidate.

Xóa vật lý ngay có thể phá provenance.

Do đó system nên hỗ trợ archive/soft removal khi cần.

# **85. Character Removal**

Nếu Character không còn dùng:

có thể đánh dấu:

ARCHIVED

thay vì xóa toàn bộ lịch sử.

# **86. Scene Removal**

Scene từng có generated outputs nên có khả năng archive/remove khỏi active storyline mà vẫn giữ lịch sử khi cần.

Chi tiết version/timeline thuộc DOMAIN-02.

# **87. Domain IDs**

Mọi core entity phải có stable internal ID.

Ví dụ:

ProjectId

StoryId

CharacterId

SceneId

CandidateId

AssetId

Display name không được dùng thay ID.

# **88. Domain Naming**

Tên có thể thay đổi.

ID không thay đổi.

Ví dụ:

CharacterId = C-001

Name:

Alice

↓

Alicia

vẫn là cùng Character.

# **89. Domain Status Pattern**

Các entity có lifecycle nên dùng explicit status thay vì suy đoán.

Ví dụ:

ACTIVE

ARCHIVED

thay vì chỉ dựa vào:

deletedAt == null

ở tầng nghiệp vụ.

Database implementation có thể khác.

# **90. Story Core Acceptance Criteria**

### **AC-DOM-01**

Given một Project mới,  
when user tạo Story,  
then Story phải có Story Core container ngay cả khi phần lớn field còn trống.

### **AC-DOM-02**

Given AI đề xuất một Story Fact,  
when creator chưa chấp nhận,  
then Fact không được xem là canonical.

### **AC-DOM-03**

Given Character xuất hiện trong nhiều Scene,  
then mỗi Scene có thể chứa appearance/state khác nhau mà không overwrite Stable Identity.

### **AC-DOM-04**

Given Character đổi tên,  
then Character ID phải giữ nguyên.

### **AC-DOM-05**

Given user chọn một generated image,  
then Scene tham chiếu Candidate đó nhưng các chi tiết ngẫu nhiên trong ảnh không tự động trở thành Story Facts.

### **AC-DOM-06**

Given Character có locked eye color,  
when AI sinh output sai eye color,  
then canonical Character data không được thay đổi.

### **AC-DOM-07**

Given Scene chứa Character A và B,  
then system phải có khả năng xác định A và B là active characters của Scene.

# **91. Dependencies**

DOMAIN-01 phụ thuộc vào:

- PROD-01 — Product Requirements;

- PROD-02 — Functional Workflows;

- ARCH-01 — Solution Architecture.

DOMAIN-01 là nguồn đầu vào quan trọng cho:

- DOMAIN-02;

- DATA-01;

- DATA-02;

- API-01;

- AI-02;

- QA documents.

# **92. Traceability**

Ví dụ traceability:

PROD Character Consistency

↓

DOMAIN Character Stable Identity

↓

SceneCharacter State

↓

AI-02 Context Selection

↓

AI-03 Consistency Validation

↓

QA-02 Evaluation

Một requirement product có thể truy xuyên toàn bộ kiến trúc.

# **93. Assumptions**

### **ASM-01**

MVP tập trung vào một creator cho mỗi Project.

### **ASM-02**

Một Project có một Story chính.

### **ASM-03**

Scene là đơn vị generation chính.

### **ASM-04**

Character identity consistency chỉ cần duy trì trong Story/Project hiện tại.

### **ASM-05**

Chapter là optional.

### **ASM-06**

AI-generated content cần được phân biệt với canonical content.

# **94. Out of Scope**

DOMAIN-01 không mô tả chi tiết:

- timeline calculation;

- story branching;

- version history;

- stale propagation;

- rollback;

- temporal character state;

- event chronology.

Các nội dung đó thuộc:

**DOMAIN-02 — Story State, Timeline, Canon & Versioning Specification.**

Ngoài ra không bao gồm:

- database schema;

- API endpoint;

- AI prompt;

- model algorithm;

- UI design.

# **95. Domain Rules Summary**

Các rule quan trọng nhất cần nhớ:

**1. Project là workspace cao nhất.**

**2. Story Core là nguồn sự thật có cấu trúc của Story.**

**3. Character được nhận diện bằng ID, không phải tên.**

**4. Stable Identity và Scene Appearance là hai thứ khác nhau.**

**5. SceneCharacter lưu trạng thái của Character trong Scene.**

**6. Generated Candidate không mặc định là canonical.**

**7. User chọn output không có nghĩa mọi chi tiết AI tạo đều trở thành Story Fact.**

**8. AI không được tự động sửa Canonical Data.**

# **96. Final Domain Model**

Có thể hình dung toàn bộ DOMAIN-01 bằng mô hình:

USER

↓

PROJECT

↓

STORY

├── STORY CORE

│ ├── Narrative Foundation

│ ├── World

│ ├── Creative Direction

│ └── Story Facts

│

├── CHARACTERS

│ ├── Stable Identity

│ ├── Base Appearance

│ ├── References

│ └── Relationships

│

└── SCENES

├── Scene Content

├── Location

├── Scene Characters

│ └── Scene-specific State

│

└── Generated Candidates

↓

Selected Output

Giải thích bằng lời:

Creator tạo Project và phát triển một Story. Story Core giữ những quyết định cốt lõi về câu chuyện và thế giới. Character có identity riêng và có thể xuất hiện trong nhiều Scene. Khi Character đi vào một Scene, SceneCharacter lưu trạng thái riêng của nhân vật tại khoảnh khắc đó. AI sử dụng các dữ liệu này để tạo Candidate, nhưng Candidate vẫn nằm ngoài Canon cho đến khi workflow phù hợp chấp nhận thay đổi.

# **97. Core Principle**

Nguyên tắc cuối cùng của DOMAIN-01:

> **OWNIVERSE does not treat the generated image as the story.  
> The structured story is the truth; the generated image is an interpretation of that truth.**

Nói đơn giản:

**Câu chuyện tồn tại trước. AI chỉ diễn giải câu chuyện đó thành nội dung và hình ảnh.**

Điều này chính là nền tảng để OWNIVERSE có thể giải quyết bài toán **character consistency và long-range story consistency** một cách có hệ thống.
