# **DOMAIN-02 — Story State, Timeline, Canon & Versioning Specification**

**Document ID:** DOMAIN-02  
**Document Type:** Domain State & Versioning Specification  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Primary Role:** Domain Architect / Business Analyst  
**Status:** Draft v1  
**Parent Documents:** PROD-01, PROD-02, ARCH-01, DOMAIN-01  
**Related Documents:** DATA-01, DATA-02, API-01, AI-02, AI-03, QA-01, QA-02

## **1. Purpose**

Tài liệu này định nghĩa cách OWNIVERSE quản lý **trạng thái của câu chuyện theo thời gian**.

DOMAIN-01 đã trả lời:

> Một Story gồm những thành phần nào?

DOMAIN-02 trả lời:

> Khi Story phát triển từ Scene 1 đến Scene 50, hệ thống biết điều gì đã xảy ra, điều gì đang đúng, Character đang ở trạng thái nào, dữ liệu nào đã thay đổi và output nào đã cũ bằng cách nào?

Tài liệu tập trung vào:

- story state;

- chronology;

- event;

- canon;

- character state transition;

- world state;

- story fact lifecycle;

- versioning;

- dependency;

- stale detection;

- branching boundary;

- rollback semantics.

# **2. Core Principle**

Nguyên tắc trung tâm:

> **Story consistency requires knowing not only what is true, but when it became true.**

Ví dụ:

Scene 1:

A does not know B is her brother.

Scene 12:

A discovers B is her brother.

Scene 20:

A already knows this fact.

Nếu hệ thống chỉ lưu:

A knows B is her brother = true

thì khi generate Scene 5, AI có thể sử dụng kiến thức chưa xảy ra.

Vì vậy OWNIVERSE phải hiểu **thời điểm và phạm vi hiệu lực của Story State**.

# **3. Domain Concepts**

DOMAIN-02 sử dụng các khái niệm chính:

Story Timeline

Story Event

Story Fact

Canon State

Character State

World State

Scene State

State Transition

Version

Snapshot

Dependency

Stale Output

Các khái niệm này liên kết với nhau nhưng không phải một thứ.

# **4. Story Timeline**

## **4.1 Definition**

**Story Timeline** là thứ tự logic của các Scene và Event trong câu chuyện.

Timeline không nhất thiết giống thứ tự user tạo Scene.

# **5. Narrative Order vs Creation Order**

Hai khái niệm phải tách biệt.

Ví dụ user có thể tạo:

Scene 10

trước rồi mới bổ sung:

Scene 5

Nhưng timeline vẫn là:

Scene 5

→ Scene 6

→ ...

→ Scene 10

Do đó:

CreatedAt

≠

Story Order

# **6. Scene Order**

Mỗi Scene phải có explicit ordering information.

Ví dụ:

SceneOrder = 12

hoặc một ordering mechanism tương đương.

Không dùng timestamp để suy ra chronology.

# **7. Narrative Time**

Một Scene có thể có thêm thông tin về thời gian trong thế giới truyện.

Ví dụ:

Day 1

Day 2

Three months later

Year 2035

Narrative Time là optional trong MVP.

Scene Order vẫn là cơ chế bắt buộc.

# **8. Flashback and Non-Linear Storytelling**

Story có thể kể không tuyến tính.

Ví dụ:

Scene 10

Narrative Event Time = 10 years ago

Do đó cần phân biệt:

Narrative Presentation Order

và:

In-World Chronological Time

MVP có thể ưu tiên Presentation Order trước.

Advanced chronology có thể mở rộng sau.

# **9. Story Event**

## **9.1 Definition**

**Story Event** biểu diễn một sự kiện có ý nghĩa làm thay đổi Story State.

Ví dụ:

- Character discovers a secret;

- Character loses an item;

- city is destroyed;

- relationship changes;

- Character is injured;

- Character changes allegiance.

# **10. Event vs Scene**

Scene là đơn vị kể chuyện.

Event là điều xảy ra bên trong hoặc giữa các Scene.

Một Scene có thể chứa:

0..N Story Events

# **11. Example**

Scene 7:

A enters the castle.

A meets B.

B reveals the truth.

Có thể tạo Events:

Event 1:

A enters Castle X.

Event 2:

A meets B.

Event 3:

A learns Fact Y.

Không nhất thiết mỗi câu văn là một Event.

Chỉ những thay đổi có ý nghĩa đối với continuity mới cần Event.

# **12. Event Core Structure**

Logical Event:

StoryEvent

\- EventId

\- StoryId

\- SceneId

\- EventType

\- Description

\- Subjects

\- Targets

\- ResultingChanges

\- StoryOrder

\- Status

Đây chưa phải database schema.

# **13. Event Subject**

Subject là entity thực hiện hoặc chịu tác động chính.

Ví dụ:

A gives sword to B

Có thể biểu diễn:

Subject = A

Target = B

Object = Sword

# **14. Event Types**

Các nhóm Event có thể gồm:

DISCOVERY

LOCATION_CHANGE

RELATIONSHIP_CHANGE

ITEM_CHANGE

STATUS_CHANGE

WORLD_CHANGE

APPEARANCE_CHANGE

CUSTOM

Không cần khóa domain chỉ vào các enum này.

# **15. Story Fact Lifecycle**

DOMAIN-01 đã định nghĩa Story Fact.

DOMAIN-02 bổ sung lifecycle.

Một Fact có thể:

PROPOSED

ACTIVE

SUPERSEDED

RETIRED

INVALIDATED

# **16. Active Fact**

**ACTIVE** nghĩa là Fact hiện đang được xem là canonical trong phạm vi hiệu lực của nó.

# **17. Superseded Fact**

Ví dụ:

Ban đầu:

A believes the king is alive.

Sau Scene 14:

A learns the king is dead.

Fact trước đó không nhất thiết bị xóa.

Nó có thể trở thành:

SUPERSEDED

vì nó đúng ở một giai đoạn trước.

# **18. Historical Truth vs Current Truth**

Một Story cần giữ được cả:

What was true then?

và:

What is true now?

Đây là lý do không overwrite toàn bộ State mỗi lần thay đổi.

# **19. Fact Effective Range**

Một Fact có thể có:

EffectiveFromScene

EffectiveUntilScene

Ví dụ:

A owns the sword

From Scene 4

Until Scene 11

Sau Scene 11:

B owns the sword

# **20. Canon**

## **20.1 Definition**

**Canon** là tập những dữ liệu được hệ thống và creator xem là sự thật chính thức của Story.

Canon không phải một object duy nhất.

Nó là tập hợp các canonical entities và facts.

# **21. Canon Categories**

Có thể chia:

Global Canon

Character Canon

World Canon

Relationship Canon

Scene Canon

Temporal Canon

# **22. Global Canon**

Ví dụ:

- genre;

- central world rules;

- story premise;

- immutable world constraints.

# **23. Character Canon**

Ví dụ:

- identity;

- permanent physical attributes;

- biography facts;

- core personality traits.

# **24. Temporal Canon**

Temporal Canon là dữ liệu đúng ở một khoảng thời gian cụ thể.

Ví dụ:

Scene 1–8:

A is injured.

Scene 9 onward:

A has recovered.

# **25. Canon Mutation Rule**

Canonical data không được thay đổi trực tiếp bởi AI.

Luồng đúng:

AI Proposal

↓

Review / Rule

↓

Accept

↓

Canonical Update

# **26. Canon Conflict**

Canon Conflict xảy ra khi hai canonical facts không thể cùng đúng trong cùng scope/time.

Ví dụ:

A has blue eyes.

A has green eyes.

cùng thời điểm và đều được đánh dấu locked.

System phải phát hiện xung đột.

# **27. Character State**

## **27.1 Definition**

Character State là trạng thái của Character tại một vị trí cụ thể trong Story.

Character State khác Stable Identity.

# **28. Character State Categories**

Có thể gồm:

Location

Appearance

Outfit

Emotion

Physical Condition

Inventory

Knowledge

Relationship State

Narrative Status

# **29. Example Character State**

Scene 3:

A:

Location = Village

Outfit = School Uniform

Emotion = Curious

Injury = None

Knows Secret X = No

Owns Sword = No

Scene 12:

A:

Location = Capital

Outfit = Travel Coat

Emotion = Angry

Injury = Left Arm

Knows Secret X = Yes

Owns Sword = Yes

Stable Identity vẫn là cùng một Character.

# **30. State Transition**

Một Event có thể tạo State Transition.

Previous State

↓

Story Event

↓

New State

# **31. Example Transition**

Before Scene 8:

Sword Owner = A

Event:

A gives sword to B

After Event:

Sword Owner = B

# **32. State Transition Rule**

## **DOM2-ST-01**

Một transition không được thay đổi các thuộc tính ngoài phạm vi Event nếu không có rule hoặc user action tương ứng.

Ví dụ:

A đổi áo không được khiến Eye Color đổi.

# **33. Derived State**

Một số State có thể được suy ra từ Event history.

Ví dụ:

A travels from City X to City Y.

Sau Event:

Current Location = City Y

# **34. Stored vs Derived State**

Không phải mọi State đều cần lưu trực tiếp.

Có hai dạng:

Stored State

Derived State

Exact implementation thuộc DATA-01.

Domain chỉ yêu cầu kết quả phải xác định được.

# **35. Scene Entry State**

Mỗi Scene có thể cần biết:

> Character ở trạng thái nào khi Scene bắt đầu?

Đây gọi là:

Scene Entry State

# **36. Scene Exit State**

Sau Scene:

Scene Exit State

có thể khác Entry State.

Ví dụ:

Entry:

A is healthy.

During Scene:

A is injured.

Exit:

A has injured left arm.

Scene sau phải nhận State mới.

# **37. State Propagation**

State có thể propagate sang Scene tiếp theo.

Scene 10 Exit State

↓

Scene 11 Entry State

Nhưng chỉ đối với thuộc tính có tính continuity.

# **38. Non-Propagating State**

Không phải tất cả State đều tự động propagate.

Ví dụ:

Pose = Sitting

không có nghĩa Scene sau Character vẫn ngồi.

Trong khi:

Injury = Broken Arm

có thể tiếp tục.

# **39. State Persistence Classification**

State field có thể được phân loại:

Persistent

Temporary

Scene-Only

Derived

# **40. Persistent State**

Ví dụ:

- injury lasting multiple scenes;

- possession of an item;

- relationship state;

- knowledge gained.

# **41. Temporary State**

Ví dụ:

- wet clothes;

- temporary emotion;

- dirt;

- short-term disguise.

# **42. Scene-Only State**

Ví dụ:

- pose;

- current action;

- gaze direction.

# **43. Knowledge State**

Đây là phần quan trọng đối với long-range storytelling.

Character Knowledge phải phân biệt:

What is true in the Story

và:

What Character A knows

Hai thứ không giống nhau.

# **44. Example Knowledge**

Canon:

B is the masked knight.

Scene 1–9:

A does not know this.

Scene 10:

A discovers it.

Nếu generate Scene 5, AI không được cho A hành xử như đã biết.

# **45. Character Knowledge Fact**

Có thể biểu diễn logical relation:

Character

↓

Knowledge Fact

↓

KnownFromScene

Exact storage thuộc Data group.

# **46. Relationship State**

Relationship cũng có thể thay đổi theo Timeline.

Ví dụ:

Scene 1:

A distrusts B.

Scene 8:

A begins trusting B.

Scene 20:

A considers B a close friend.

Relationship không nên chỉ là một label cố định.

# **47. Relationship Transition**

Một relationship có thể có:

Type

Strength

Direction

Effective Range

Ví dụ:

A → B

Trust = LOW

later

Trust = HIGH

# **48. World State**

Story World cũng thay đổi.

Ví dụ:

City = intact

sau Event:

City = destroyed

Scene sau không được generate city như chưa từng bị phá nếu canon yêu cầu continuity.

# **49. Location State**

Location có thể có State:

- weather;

- damage;

- ownership;

- accessibility;

- population state;

- visual change.

# **50. Object State**

Nếu Story sử dụng item quan trọng:

Sword

Book

Key

Vehicle

Artifact

System có thể cần biết:

Owner

Location

Condition

Status

MVP có thể hỗ trợ thông qua Story Facts thay vì tạo full Item Domain ngay.

# **51. Scene Canon**

Scene Canon bao gồm những gì creator xác nhận đã thực sự xảy ra trong Scene.

Ví dụ AI generate image cho Scene:

A and B walking in rain.

Nếu AI vô tình thêm:

red umbrella

thì red umbrella không tự động thành Scene Canon.

# **52. Canon Extraction Rule**

Không tự động extract mọi visual detail từ generated output vào Canon.

AI có thể đề xuất:

Potential Fact

nhưng cần explicit acceptance.

# **53. Story Versioning**

## **53.1 Definition**

Versioning cho phép hệ thống biết dữ liệu canonical đã thay đổi qua các lần nào.

Versioning không đồng nghĩa với Git.

# **54. Entity Version**

Các entity quan trọng nên có logical version.

Ví dụ:

Character A

Version 1

Character A

Version 2

CharacterId vẫn giữ nguyên.

# **55. Version Increment**

Version tăng khi canonical data thay đổi có ý nghĩa.

Ví dụ:

Character description changed

→ new version

Không nhất thiết tăng version cho mọi analytics metadata.

# **56. Scene Version**

Scene cũng có thể có version.

Ví dụ:

Scene 12 v1

sau khi creator đổi:

Location

Active Characters

Description

thành:

Scene 12 v2

# **57. Story Core Version**

Story Core cũng cần version.

Ví dụ:

StoryCore v7

Generation Job có thể tham chiếu:

StoryCore v7

Character A v3

Scene 12 v4

# **58. Why Versioning Matters**

Version giúp trả lời:

> Candidate này được generate dựa trên dữ liệu nào?

Ví dụ:

Candidate X:

Character A v3

Current:

Character A v5

Candidate X có thể không còn phù hợp.

# **59. Snapshot**

## **59.1 Definition**

Snapshot là representation bất biến của context tại thời điểm generation.

Version nói:

> entity là version nào.

Snapshot nói:

> AI đã thực sự nhận bộ context nào.

# **60. Snapshot Example**

Generation Context Snapshot

StoryCore v7

Scene 12 v4

Character A v3

Character B v6

Visual Direction v2

Snapshot không thay đổi sau khi job bắt đầu.

# **61. Snapshot Rule**

## **DOM2-VR-01**

Running generation SHALL continue using its initial Snapshot even if canonical data changes during execution.

# **62. Dependency**

Generated output phụ thuộc vào những canonical versions được dùng khi generate.

Logical:

Candidate X

↓

Dependency Set

├── StoryCore v7

├── Scene 12 v4

├── Character A v3

└── Character B v6

# **63. Dependency Purpose**

Dependency cho phép xác định:

- output nào bị ảnh hưởng khi Character đổi;

- candidate nào được tạo bằng context cũ;

- có nên cảnh báo user;

- có cần regenerate hay không.

# **64. Stale Output**

## **64.1 Definition**

Một Candidate là **stale** khi canonical context liên quan đã thay đổi kể từ lúc Candidate được tạo.

# **65. Example Stale Case**

Candidate X dùng:

Character A v2

User thay Character thành:

Character A v3

Nếu thay đổi liên quan tới generated image:

Candidate X = STALE

# **66. Not Every Change Makes Output Stale**

Ví dụ Character đổi:

Biography note

nhưng Candidate chỉ là close-up portrait và field đó không ảnh hưởng visual generation.

System không nhất thiết phải stale mọi Candidate.

# **67. Dependency-Aware Stale Detection**

Tốt hơn:

Changed Field

↓

Affected Dependency Type

↓

Related Candidates

↓

Mark Potentially Stale

MVP có thể đơn giản hóa theo entity version.

# **68. Stale Severity**

Có thể chia:

INFO

REVIEW_RECOMMENDED

REGENERATION_RECOMMENDED

INVALID

Ví dụ:

Character name changed:

INFO

Character face reference changed:

REGENERATION_RECOMMENDED

# **69. Stale Does Not Mean Delete**

Candidate cũ không bị xóa.

Nó vẫn cần cho:

- history;

- comparison;

- research;

- provenance.

# **70. Selected Candidate Becoming Stale**

Nếu selected candidate trở thành stale:

Scene không nhất thiết mất selected output ngay.

UI có thể hiện:

> Generated with older character data.

User có thể:

- giữ;

- review;

- regenerate;

- select candidate khác.

# **71. Canon Update Propagation**

Khi canonical entity thay đổi:

Canonical Update

↓

Create New Version

↓

Publish Domain Event

↓

Dependency Analysis

↓

Find Outputs

↓

Mark Relevant Outputs Stale

# **72. Character Update Example**

User đổi:

A Hair Color

Black → Silver

Nếu đây là locked visual attribute:

mọi generated Candidate liên quan tới Character A từ version cũ có thể được đánh dấu:

REVIEW_RECOMMENDED

hoặc:

REGENERATION_RECOMMENDED

# **73. Story Core Change Example**

User đổi:

Visual Style

Anime watercolor

→ Dark cinematic manga

Không nên xóa ảnh cũ.

Nhưng image candidates dựa trên style cũ có thể stale về creative direction.

# **74. Scene Change Example**

User đổi Scene 20:

Location = Forest

thành:

Location = Castle

Image hiện tại của Scene 20 gần như chắc chắn:

REGENERATION_RECOMMENDED

# **75. Version vs Revision**

Có thể phân biệt:

**Revision**

Một lần edit.

**Version**

Một canonical state đáng lưu/truy dấu.

MVP có thể coi hai khái niệm giống nhau để đơn giản.

# **76. Version Immutability**

Một version đã được generation sử dụng nên được xem như immutable historical record.

Không chỉnh sửa ngược version cũ.

Tạo version mới.

# **77. Rollback**

Rollback không nên xóa lịch sử.

Ví dụ:

Current Character = v5

User muốn quay lại v3.

Không cần rewrite history thành v3.

Có thể tạo:

v6 = restored state from v3

Nhờ đó provenance vẫn tuyến tính.

# **78. Why Restore Creates New Version**

Nếu ta đơn giản biến:

CurrentVersion = v3

thì khó xác định chronology của edit history.

Tạo v6 giúp biết:

v1 → v2 → v3 → v4 → v5 → v6(restored from v3)

# **79. Branching**

Story branching có thể hữu ích cho:

- alternate ending;

- what-if;

- experimental storyline.

Nhưng không phải MVP requirement bắt buộc.

# **80. MVP Branching Decision**

Trong MVP:

One canonical storyline

được ưu tiên.

Không cần full branching model.

Nếu user muốn thử alternative:

có thể dùng:

- candidate;

- duplicate scene/project;

- future branching feature.

# **81. Future Branch Model**

Nếu sau này hỗ trợ:

Story

├── Main Timeline

├── Branch A

└── Branch B

mỗi branch sẽ có Canon State riêng sau divergence point.

Không cần triển khai hiện tại.

# **82. Scene Insertion**

Nếu user chèn Scene mới vào giữa:

Scene 10

Scene 11

thành:

Scene 10

Scene 10.5

Scene 11

thì state propagation phía sau có thể bị ảnh hưởng.

# **83. Reordering Scenes**

Reorder có thể làm thay đổi continuity.

Ví dụ Event gây injury ban đầu ở Scene 8.

User di chuyển Scene đó xuống sau Scene 12.

Các Scene 9–12 có thể không còn hợp logic.

Hệ thống nên đánh dấu affected continuity để review.

# **84. Timeline Mutation Rule**

Timeline edit phải được xem là canonical structural change.

Có thể trigger:

Continuity Review Required

cho downstream scenes.

# **85. Downstream Impact**

Một Event thay đổi ở Scene N có thể ảnh hưởng:

Scene N+1

Scene N+2

...

nhưng không phải luôn toàn bộ Story.

# **86. Impact Boundary**

Impact có thể dừng khi:

- State bị overwrite bởi Event sau;

- Fact hết hiệu lực;

- Character không còn liên quan;

- explicit reset xảy ra.

Advanced dependency reasoning thuộc AI/application layer.

# **87. Consistency Check Boundary**

DOMAIN-02 xác định consistency expectation.

Ví dụ:

If A lost sword in Scene 5,

A should not hold that sword in Scene 8

unless another Event returns it.

AI-02/AI-03 quyết định cách kiểm tra tự động.

# **88. Long-Range Continuity**

Long-range consistency nghĩa là thông tin quan trọng vẫn được giữ qua khoảng cách Scene dài.

Ví dụ:

Scene 3:

A receives scar.

Scene 40:

Scar should still exist.

nếu không có Event loại bỏ/thay đổi nó.

# **89. Identity Continuity**

Identity continuity có priority cao.

Ví dụ:

Character A identity

phải nhất quán xuyên suốt Story bất kể:

- outfit;

- location;

- emotion;

- scene distance.

# **90. Appearance Continuity**

Appearance có thể thay đổi hợp lệ.

Do đó system phải phân biệt:

Identity Drift

với:

Legitimate Appearance Change

Ví dụ đổi áo không phải identity inconsistency.

# **91. State Conflict**

State Conflict xảy ra khi cùng một moment có hai state không thể cùng đúng.

Ví dụ:

A Location = Tokyo

A Location = Paris

trong cùng Scene, trừ khi domain cho phép lý do đặc biệt.

# **92. Conflict Resolution**

System không nên tự đoán khi conflict canonical quan trọng.

Workflow:

Detect Conflict

↓

Present Conflict

↓

User / Explicit Rule Resolves

↓

Create New Canonical Version

# **93. AI Conflict Suggestion**

AI có thể đề xuất giải pháp.

Nhưng không tự động sửa Canon nếu không có permission/rule rõ ràng.

# **94. Incomplete Timeline**

Story có thể chưa hoàn thiện.

Ví dụ:

Scene 1

Scene 2

Scene 5

Scenes 3–4 chưa tồn tại.

Domain vẫn hợp lệ.

System không cần giả định missing Scene.

# **95. Draft Scene State**

Scene draft có thể chưa contribute toàn bộ facts vào Canon nếu creator chưa xác nhận.

Có thể phân biệt:

DRAFT

CONFIRMED

ARCHIVED

# **96. Confirmed Scene**

Confirmed Scene nghĩa là creator xem narrative content hiện tại là phần active của canonical storyline.

Điều này không có nghĩa Scene không thể edit nữa.

Edit sẽ tạo version mới.

# **97. Draft Event**

AI hoặc system có thể extract Event từ Scene và đặt:

PROPOSED

Creator xác nhận mới thành canonical event nếu workflow dùng extraction.

# **98. Event Extraction Boundary**

OWNIVERSE có thể hỗ trợ:

Scene Text

↓

AI Event Extraction

↓

Proposed Events

↓

Review

↓

Canonical Events

Không bắt buộc trong MVP ban đầu.

# **99. Canon Commit**

Một canonical change nên có metadata:

Changed Entity

Previous Version

New Version

Change Source

Changed By

Changed At

# **100. Change Source**

Source có thể là:

USER_EDIT

AI_PROPOSAL_ACCEPTED

SYSTEM_ACTION

RESTORE

IMPORT

Điều này hỗ trợ provenance.

# **101. User Edit Priority**

Explicit user edit có priority cao hơn AI assumption.

Nếu conflict:

User Canonical Decision

\>

AI Generated Assumption

# **102. Lock Interaction**

Locked canonical attribute có semantics mạnh hơn version đơn thuần.

Ví dụ:

Eye Color = Blue \[LOCKED\]

AI proposal muốn:

Green

Proposal phải bị reject hoặc cảnh báo mạnh.

Không update tự động.

# **103. Unlock**

Creator có thể chủ động unlock và thay đổi attribute.

Ví dụ:

Hair Color:

Black \[LOCKED\]

User unlocks

→ changes to White

Sau đó new canonical version được tạo.

Old generations có thể stale.

# **104. Permanent Character Change**

Một Character có thể thay đổi identity-defining trait vì Story.

Ví dụ:

A loses left eye.

Đây không phải AI inconsistency nếu Story Event xác nhận thay đổi.

Do đó Stable Identity cũng có thể có temporal evolution trong một số trường hợp.

# **105. Identity Evolution**

Thay vì coi Stable Identity tuyệt đối bất biến:

Stable Identity

nên hiểu là:

> Identity-defining attributes that remain stable unless an explicit canonical Event changes them.

Điều này quan trọng cho long stories.

# **106. Identity Change Event**

Ví dụ:

Scene 25:

A cuts hair short.

Trước Scene 25:

Long hair

Sau Scene 25:

Short hair

Generate Scene 10 phải dùng long hair.

Generate Scene 30 phải dùng short hair.

# **107. Character Reference Version**

Visual Reference cũng cần version hoặc effective range.

Ví dụ:

Reference Set v1

Scenes 1–24

Reference Set v2

Scenes 25+

Điều này hỗ trợ intentional appearance evolution.

# **108. Reference Selection by Scene**

AI Context Builder có thể chọn Character Reference phù hợp theo Scene position.

Logic chi tiết thuộc AI-02.

DOMAIN-02 chỉ xác định requirement:

> reference selection phải có khả năng phản ánh canonical temporal state.

# **109. Scene State Snapshot**

Mỗi Scene có thể tạo logical State Snapshot dùng cho generation.

Ví dụ:

Scene 30 State

A:

Hair = Short

Outfit = Black Coat

Knowledge X = Known

B:

Location = Castle

Injury = None

World:

Castle = Damaged

# **110. Current Story State**

**Current Story State** là state tại điểm cuối canonical timeline hiện tại.

Nó không nên dùng mù quáng cho Scene cũ.

# **111. Historical Scene Query**

System phải có khả năng về mặt domain trả lời:

> What was Character A's state at Scene 8?

không chỉ:

> What is Character A's current state?

Đây là requirement quan trọng.

# **112. Temporal Query Requirements**

Hệ thống cần hỗ trợ khái niệm:

StateAt(sceneId)

FactsAt(sceneId)

CharacterStateAt(characterId, sceneId)

WorldStateAt(sceneId)

Không quy định implementation API tại đây.

# **113. Candidate Temporal Context**

Candidate phải gắn với Scene-time context.

Ví dụ:

Candidate X

Generated for Scene 8

using CharacterStateAt(Scene 8)

Không dùng Current Story State nếu Scene 8 nằm trong quá khứ.

# **114. Regeneration of Old Scene**

Nếu user regenerate Scene 8 sau khi Story đã phát triển đến Scene 50:

AI phải dùng Canon tại Scene 8.

Không được dùng Character State hiện tại ở Scene 50 nếu State đó chưa tồn tại tại Scene 8.

# **115. Critical Long-Range Rule**

## **DOM2-LR-01**

Generation for a Scene SHALL use the canonical state effective at that Scene, not simply the latest global state.

Đây là một trong các rule quan trọng nhất của toàn bộ thesis application.

# **116. Example**

Story:

Scene 1:

A has long hair.

Scene 20:

A cuts hair short.

Scene 40:

Current state = short hair.

User regenerate Scene 5.

Sai:

Use current short hair.

Đúng:

Use Scene 5 state = long hair.

# **117. Current vs Effective State**

Phải phân biệt:

Current State

và:

Effective State At Scene

Hai khái niệm chỉ giống nhau ở Scene cuối nếu không có branching.

# **118. Story Recalculation**

Nếu Scene/Event cũ thay đổi:

downstream State có thể cần recalculation.

Ví dụ user xóa Event:

A gets sword at Scene 4

thì các State:

A owns sword

ở Scene sau có thể không còn đúng.

# **119. State Rebuild**

System nên có logical capability:

Base Canon

\+

Timeline Events

↓

Rebuild Effective State

Exact algorithm thuộc implementation.

# **120. Incremental State Update**

Không nhất thiết rebuild toàn Story mỗi lần.

Có thể recalculation từ Scene bị thay đổi trở đi.

MVP implementation có thể đơn giản hơn nếu dataset nhỏ.

# **121. History Preservation**

Khi recalculation xảy ra:

historical versions không được xóa.

Generation cũ vẫn cần truy xuất provenance.

# **122. Version Retention**

Các version đã từng được:

- generation sử dụng;

- selected output tham chiếu;

- research experiment dùng;

nên được giữ.

# **123. Garbage Collection Boundary**

Version chưa bao giờ được sử dụng và rất cũ có thể được cleanup trong tương lai.

Không phải requirement MVP.

# **124. Canonical Change Impact Categories**

Canonical changes có thể phân loại:

NO_GENERATION_IMPACT

TEXTUAL_IMPACT

VISUAL_IMPACT

IDENTITY_IMPACT

TIMELINE_IMPACT

GLOBAL_IMPACT

# **125. Identity Impact**

Ví dụ:

Face reference changed

có thể ảnh hưởng tất cả future image generations của Character.

# **126. Timeline Impact**

Ví dụ:

Scene reordered

có thể ảnh hưởng state propagation của downstream scenes.

# **127. Global Impact**

Ví dụ:

Entire visual style changed

có thể ảnh hưởng hầu hết image outputs.

# **128. Dependency Graph Concept**

Về mặt logical:

Canonical Entities

↓

Versions

↓

Snapshots

↓

Generation Jobs

↓

Candidates

Khi canonical entity thay đổi, system có thể đi ngược dependency để tìm Candidate bị ảnh hưởng.

# **129. Provenance Chain**

Một generated output phải truy ngược được:

Candidate

↓

Attempt

↓

Job

↓

Context Snapshot

↓

Entity Versions

↓

Canonical Story State

# **130. Story Audit History**

Creator có thể không cần xem toàn bộ technical audit.

Nhưng system phải giữ đủ dữ liệu để:

- debug;

- explain stale status;

- reproduce experiments;

- trace changes.

# **131. User-Facing History**

UI có thể đơn giản hóa thành:

Changed character appearance

Generated new image

Selected candidate

Updated scene

Không cần expose toàn bộ internal version graph.

# **132. Version Label**

Internal:

Character v17

User-facing có thể không cần thấy số version trừ khi vào history/debug.

# **133. Temporal Consistency Warning**

Ví dụ system phát hiện:

Scene 30 says A still owns sword,

but sword was lost in Scene 20.

UI nên hiển thị:

> This scene may conflict with an earlier story event.

Không hiển thị raw technical dependency error cho creator.

# **134. Canonical Conflict Severity**

Có thể chia:

WARNING

ERROR

BLOCKING

Không phải mọi continuity issue đều block user.

# **135. Blocking Example**

Generate image cho Scene nhưng Character ID không tồn tại:

BLOCKING

# **136. Warning Example**

Outfit khác Scene trước nhưng có thể hợp lệ:

WARNING

User có thể chủ động đổi outfit.

# **137. Story Completion**

Khi Project được đánh dấu Completed:

Canon vẫn có thể được chỉnh nếu user reopen.

Completed không đóng băng vĩnh viễn toàn bộ versions.

# **138. Archive**

Archived Story giữ:

- Canon;

- versions;

- timeline;

- candidates;

- provenance.

Không chạy background consistency update trừ khi reopen hoặc user yêu cầu.

# **139. Domain Requirements**

## **DOM2-FR-01**

System SHALL duy trì explicit Scene order.

## **DOM2-FR-02**

System SHALL có khả năng biểu diễn Story Event làm thay đổi State.

## **DOM2-FR-03**

System SHALL phân biệt current state và historical state.

## **DOM2-FR-04**

System SHALL có khả năng xác định Character State tại một Scene cụ thể.

## **DOM2-FR-05**

Canonical entity changes SHALL tạo logical version mới.

## **DOM2-FR-06**

Generation SHALL tham chiếu immutable Context Snapshot.

## **DOM2-FR-07**

Generated Candidate SHALL giữ dependency tới canonical versions được sử dụng.

## **DOM2-FR-08**

System SHALL có khả năng đánh dấu Candidate sử dụng context cũ.

## **DOM2-FR-09**

Stale Candidate SHALL NOT bị tự động xóa.

## **DOM2-FR-10**

Scene-specific State SHALL support temporal continuity.

## **DOM2-FR-11**

Knowledge State SHALL có khả năng thay đổi theo Timeline.

## **DOM2-FR-12**

Intentional Character appearance change SHALL có thể có hiệu lực từ một Scene cụ thể.

## **DOM2-FR-13**

Regeneration of an old Scene SHALL use State effective at that Scene.

## **DOM2-FR-14**

Rollback SHALL preserve history.

## **DOM2-FR-15**

AI-generated change SHALL NOT become Canon without accepted workflow.

# **140. Acceptance Criteria**

### **AC-DOM2-01**

Given Character A has long hair until Scene 20,  
when user regenerates Scene 5 after Scene 20 exists,  
then generation context must still describe A with long hair.

### **AC-DOM2-02**

Given A learns a secret in Scene 10,  
when Scene 5 is generated,  
then system must not treat that secret as known by A.

### **AC-DOM2-03**

Given Character A changes canonical eye color,  
when existing Candidates were generated from the previous version,  
then affected Candidates must remain accessible and may be marked stale.

### **AC-DOM2-04**

Given user edits Scene 8,  
when the edit changes a continuity-relevant Event,  
then downstream Story State must be eligible for recalculation or review.

### **AC-DOM2-05**

Given a Candidate was created from Character v3,  
when Character v4 becomes current,  
then system must still be able to identify that Candidate's dependency on v3.

### **AC-DOM2-06**

Given user restores Character state from an earlier version,  
then the restore must preserve previous version history instead of deleting intermediate versions.

### **AC-DOM2-07**

Given a Character loses an item in Scene 12,  
when generating Scene 15,  
then Character should not be considered to own that item unless another canonical Event returns it.

### **AC-DOM2-08**

Given current Story state differs from historical Scene state,  
then generation for an old Scene must use historical effective state.

### **AC-DOM2-09**

Given an image contains an unplanned object,  
then selecting that image must not automatically make the object canonical.

### **AC-DOM2-10**

Given Character identity changes intentionally through a canonical Story Event,  
then outputs before and after that Event may validly use different appearance references.

# **141. Dependencies**

DOMAIN-02 phụ thuộc vào:

- DOMAIN-01 — Project & Story Core Domain Specification;

- PROD-02 — workflow;

- ARCH-01 — architecture boundaries;

- ARCH-02 — runtime jobs and snapshots.

DOMAIN-02 cung cấp nền cho:

- DATA-01 — Data Model & Persistence;

- DATA-02 — Asset, Versioning & Storage;

- API-01 — entity contract;

- API-02 — async event contract;

- AI-02 — context/memory;

- AI-03 — generation validation;

- QA-02 — consistency evaluation.

# **142. Traceability**

Ví dụ trace:

Product Requirement:

Long-Range Character Consistency

↓

DOMAIN-01:

Character Stable Identity

↓

DOMAIN-02:

Temporal Character State

Version

Scene-effective State

↓

AI-02:

Context Retrieval

↓

AI-03:

Consistency Validation

↓

QA-02:

Long-Range Consistency Evaluation

Giải thích bằng lời:

Product yêu cầu nhân vật nhất quán trong truyện dài. DOMAIN-01 xác định Character là ai. DOMAIN-02 xác định Character ở trạng thái nào tại từng Scene. AI-02 lấy đúng trạng thái đó làm context. AI-03 kiểm tra output có phù hợp hay không. Cuối cùng QA-02 đo chất lượng consistency.

# **143. Assumptions**

### **ASM-DOM2-01**

MVP sử dụng một canonical storyline chính.

### **ASM-DOM2-02**

Scene Order là timeline ordering chính.

### **ASM-DOM2-03**

Full non-linear chronology support không bắt buộc trong MVP.

### **ASM-DOM2-04**

Mọi canonical change quan trọng có thể được versioned.

### **ASM-DOM2-05**

Candidate không bị xóa khi trở thành stale.

### **ASM-DOM2-06**

Creator có quyền quyết định Canon cuối cùng.

# **144. Out of Scope**

DOMAIN-02 không định nghĩa:

- physical version tables;

- event sourcing implementation;

- database indexes;

- API endpoints;

- actual stale detection algorithm;

- embedding-based memory;

- vector retrieval;

- consistency scoring formula;

- Git-like merge;

- collaborative conflict resolution;

- full alternate timeline branching.

# **145. Important Architecture Decision**

OWNIVERSE **không cần triển khai full Event Sourcing** chỉ vì có Timeline và Versions.

Domain yêu cầu:

- history;

- versions;

- events;

- state-at-scene capability.

Nhưng persistence implementation có thể đơn giản hơn:

Current Canonical Data

\+

Version History

\+

Story Events

\+

Snapshots

thay vì xây toàn bộ system dưới dạng event-sourced architecture.

Điều này giúp thesis giữ được độ mạnh về domain nhưng không tăng complexity quá mức.

# **146. Domain State Model**

Mô hình tổng quát:

BASE CANON

↓

STORY TIMELINE

↓

EVENT 1

↓

STATE 1

↓

EVENT 2

↓

STATE 2

↓

...

↓

CURRENT STATE

Một generation tại Scene N không nhất thiết dùng Current State.

Nó phải lấy:

STATE EFFECTIVE AT SCENE N

# **147. Generation Context Model**

Scene N

↓

Determine Effective Story State

↓

Determine Active Characters

↓

Resolve Character State at Scene N

↓

Resolve World / Location State

↓

Resolve Known Canon Facts

↓

Create Immutable Context Snapshot

↓

AI Generation

Giải thích bằng lời:

Khi AI cần tạo một Scene, hệ thống trước tiên xác định Story đang ở trạng thái nào tại chính Scene đó. Sau đó lấy Character đang xuất hiện, trạng thái của từng Character, World facts và những thông tin đã trở thành Canon trước thời điểm đó. Bộ dữ liệu này mới được đóng băng thành Snapshot để AI sử dụng.

# **148. Full State Relationship**

STORY

│

├── CANON

│ ├── Story Facts

│ ├── Character Identity

│ ├── World Rules

│ └── Relationships

│

├── TIMELINE

│ ├── Scene 1

│ │ └── Events

│ ├── Scene 2

│ │ └── Events

│ └── Scene N

│ └── Events

│

└── STATE

├── Character State

├── World State

├── Relationship State

└── Knowledge State

Timeline làm thay đổi State, trong khi Canon xác định đâu là dữ liệu được công nhận chính thức.

# **149. The Critical Thesis/Application Connection**

DOMAIN-02 có ý nghĩa đặc biệt với đề tài nghiên cứu của chúng ta.

Bài toán không chỉ là:

> “Giữ khuôn mặt Character giống nhau.”

Mà thực tế là:

> “Giữ đúng **identity**, nhưng đồng thời cho phép Character thay đổi hợp lý theo tiến trình câu chuyện.”

Ví dụ:

Same Character Identity

\+

Different Scene State

\+

Correct Historical Context

=

Valid Long-Range Consistency

Nếu chỉ ép Character giống hệt reference ban đầu ở mọi Scene, hệ thống cũng sai vì Character sẽ không thể:

- thay outfit;

- bị thương;

- già đi;

- đổi tóc;

- thay cảm xúc;

- phát triển theo Story.

Do đó OWNIVERSE phải bảo vệ **identity consistency** nhưng vẫn tôn trọng **temporal evolution**.

# **150. Final Principles**

DOMAIN-02 có thể rút lại thành sáu nguyên tắc:

**1. Canon nói điều gì là sự thật.**

**2. Timeline nói điều gì xảy ra trước và sau.**

**3. Event nói điều gì đã thay đổi.**

**4. State nói một entity đang như thế nào tại một thời điểm.**

**5. Version cho biết dữ liệu đã thay đổi qua những lần nào.**

**6. Snapshot cho biết AI đã nhìn thấy chính xác dữ liệu gì khi tạo output.**

Và rule quan trọng nhất:

> **Never generate a past scene using knowledge or state that only exists in the future.**

Đối với OWNIVERSE:

> **AI phải nhìn thấy câu chuyện đúng như nó tồn tại tại Scene đang được tạo, không đơn giản là lấy trạng thái mới nhất của toàn bộ Project.**

## **Tóm tắt Nhóm 4 — Story Core & Domain Model**

Sau DOMAIN-01 và DOMAIN-02, phần domain của OWNIVERSE đã có nền khá đầy đủ.

**DOMAIN-01** trả lời:

> Một Project và Story gồm những thành phần nào?

Ta định nghĩa Project, Story Core, Character, Stable Identity, World, Scene, SceneCharacter, Story Fact, Candidate và Canon.

**DOMAIN-02** trả lời:

> Những thành phần đó thay đổi như thế nào xuyên suốt câu chuyện?

Ta định nghĩa Timeline, Event, Character State, World State, Knowledge State, Canon theo thời gian, Version, Snapshot, Dependency và Stale Output.

Luồng tổng thể có thể nhớ:

> **Story Core xác định thế giới → Timeline làm câu chuyện tiến triển → Events làm State thay đổi → Versions giữ lịch sử → Snapshot đóng băng đúng trạng thái → AI tạo Candidate từ trạng thái đó.**
