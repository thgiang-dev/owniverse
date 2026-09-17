# **QA-02 — AI, Image & Consistency Evaluation Specification**

**Document ID:** QA-02  
**Document Type:** AI, Image & Consistency Evaluation Specification  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Status:** Draft v1  
**Primary Roles:** QA Engineer / AI Engineer / Thesis Researcher  
**Parent Document:** QA-01 — Master Test Plan & Quality Strategy  
**Related Documents:** AI-02, AI-03, DOMAIN-01, DOMAIN-02, DATA-02, RESEARCH-02, RESEARCH-04

# **1. Purpose**

QA-02 xác định cách OWNIVERSE kiểm thử và đánh giá chất lượng của AI-generated image, đặc biệt đối với:

- character identity consistency;

- long-range recurrence;

- multi-character separation;

- identity leakage;

- appearance-state alignment;

- scene continuity;

- scene intent;

- visual quality;

- consistency warning;

- candidate validation decision.

Tài liệu này không định nghĩa thuật toán generation mới và không thay thế experimental evaluation trong RESEARCH-04.

Mục tiêu của QA-02 là trả lời:

> **Hệ thống có đánh giá đúng một generated candidate là hợp lệ, có warning, nên regenerate hay phải reject hay không?**

và:

> **AI output trong các workflow thực tế của OWNIVERSE có duy trì consistency ở mức chấp nhận được hay không?**

# **2. Position in the System**

Traceability hiện tại của hệ thống là:

Product Requirement

↓

DOMAIN-01

Character Identity

↓

DOMAIN-02

Character / Scene State

↓

AI-02

Context Selection

↓

AI-03

Consistency Validation

↓

QA-02

Consistency Evaluation

Tài liệu nguồn xác định rõ AI-03 kiểm tra output có phù hợp với context hay không, còn QA-02 đo chất lượng long-range consistency.

# **3. QA-02 vs AI-03**

Hai tài liệu có vai trò khác nhau.

## **AI-03**

Chạy trong application runtime.

Nó trả lời:

> Candidate hiện tại có đủ điều kiện để application xử lý tiếp không?

AI-03 thực hiện pipeline:

Technical Validation

↓

Contract Validation

↓

Semantic Validation

↓

Consistency Validation

↓

Safety / Policy Validation

↓

Candidate Decision

## **QA-02**

Chạy trong quá trình development, testing và release evaluation.

Nó trả lời:

> Validation của AI-03 có hoạt động đúng không?

và:

> Model/pipeline hiện tại có đạt mức consistency mà sản phẩm yêu cầu không?

# **4. QA-02 vs RESEARCH-04**

Cần tách rõ hai mục tiêu.

### **QA-02**

Kiểm tra:

> sản phẩm hoạt động đủ tốt hay chưa.

### **RESEARCH-04**

Kiểm tra:

> phương pháp SAMIM có cải thiện có ý nghĩa so với baseline hay không.

Ví dụ:

SAMIM có thể tốt hơn StoryDiffusion trong RESEARCH-04 nhưng vẫn tạo quá nhiều identity errors để product threshold được coi là đạt.

Khi đó:

Research Result → Positive

Product QA → Not Ready

Hai kết luận này không mâu thuẫn.

# **5. Source Validation Categories**

AI-03 hiện xác định consistency validation gồm các nhóm chính:

- Character Identity Consistency;

- Character Separation;

- Identity Leakage;

- Appearance-State Alignment;

- Scene Continuity.

QA-02 sử dụng trực tiếp các nhóm này làm evaluation dimensions.

# **6. QA Evaluation Dimensions**

QA-02 mở rộng thành tám dimension thực hành:

Q1 Character Identity

Q2 Long-Range Identity

Q3 Character Separation

Q4 Identity Leakage

Q5 Appearance-State Alignment

Q6 Scene Continuity

Q7 Scene Intent Alignment

Q8 Visual / Image Quality

Trong đó Q1–Q6 là consistency core.

Q7–Q8 đảm bảo model không đạt consistency bằng cách hy sinh nội dung hoặc chất lượng hình ảnh.

# **7. Evaluation Unit**

QA có thể đánh giá ở bốn mức.

### **Candidate Level**

Một generated image.

### **Character Instance Level**

Một character cụ thể trong image.

### **Scene Level**

Toàn bộ scene.

### **Story Sequence Level**

Một chuỗi nhiều scene.

Long-range consistency đặc biệt cần Story Sequence Level.

# **8. Candidate Evaluation Context**

Một candidate không được đánh giá riêng lẻ mà phải được đặt cạnh context đã dùng để generation.

Evaluator tối thiểu cần biết:

Scene Description

Active Characters

Character References

Character Visual States

Relevant Previous State

Generation Candidate

Nếu không có context, không thể phân biệt:

> intentional appearance change

với:

> inconsistency.

# **9. Canonical Evaluation Source**

QA phải dùng canonical application data làm expected source.

Không dùng generated output trước đó làm “ground truth” nếu output đó chưa được user approve hoặc chưa thuộc valid history.

Expected information có thể đến từ:

- Character identity;

- Character canonical reference;

- SceneCharacter state;

- current visual state;

- canonical events;

- scene description;

- project style.

# **10. Stable Identity vs Appearance**

QA phải duy trì distinction:

Stable Identity

≠

Scene Appearance

Ví dụ:

Stable:

face identity

eye characteristics

distinctive scar

core physical traits

Mutable:

outfit

expression

pose

temporary injury

wet/dry hair state

equipment

Một outfit change hợp lệ không được tự động đánh dấu là identity inconsistency.

# **11. Character Identity Consistency**

Character Identity Consistency trả lời:

> Generated character có còn được nhận ra là đúng character đã chỉ định hay không?

Có thể kiểm tra dựa trên:

- canonical reference;

- approved previous appearances;

- stable identity descriptors;

- model-assisted similarity;

- human inspection.

Exact algorithm thuộc AI/Research implementation, không được QA-02 tự giả định nếu chưa được chọn.

# **12. Identity Evaluation Cases**

Tối thiểu phải có:

### **ID-01 — Same Character, Similar Context**

Character xuất hiện ở hai scene gần nhau.

Expected:

> identity giữ ổn định.

### **ID-02 — Same Character, Different Pose**

Pose thay đổi nhưng identity giữ nguyên.

### **ID-03 — Different Camera Angle**

Camera/viewpoint thay đổi nhưng character vẫn recognizable.

### **ID-04 — Lighting Change**

Lighting thay đổi không được biến character thành identity khác.

# **13. Long-Range Identity Consistency**

Long-range consistency nghĩa là thông tin quan trọng vẫn được giữ qua khoảng cách Scene dài. Domain hiện tại cũng xác định yêu cầu này rõ ràng.

QA scenario:

Scene 1 → Character A

Scene 2 → no A

Scene 3 → no A

...

Scene 10 → Character A

Expected:

> A ở Scene 10 vẫn giữ identity của A.

# **14. Long-Range Distance**

QA record phải lưu:

previousAppearanceScene

currentScene

reappearanceGap

Generation metadata hiện tại cũng đã dự kiến lưu scene position và distance từ lần xuất hiện trước.

Không nên hard-code khái niệm “long” bằng một threshold khoa học trong QA-02.

Research bucket chính thức thuộc RESEARCH-04.

# **15. Long-Range QA Buckets**

Đối với QA regression, có thể sử dụng các nhóm logic:

Near Reappearance

Mid-Story Reappearance

Long Reappearance

Các label này dùng để tổ chức test suite.

Không được hiểu là scientific gap bucket của RESEARCH-04.

# **16. Character Separation**

Character Separation kiểm tra:

> hai character khác nhau có giữ được visual distinction hay không.

Ví dụ:

Scene:

Aria + Kael

Expected:

Aria remains Aria

Kael remains Kael

Không acceptable nếu hai người bắt đầu có cùng face hoặc distinctive traits bị hòa trộn.

# **17. Multi-Character Test**

Standard test:

Reference A

Reference B

↓

Scene contains A + B

↓

Generation

Kiểm tra:

- A có xuất hiện;

- B có xuất hiện;

- A đúng identity;

- B đúng identity;

- không swap;

- không merge;

- scene intent đúng.

# **18. Identity Leakage**

Identity Leakage xảy ra khi visual information thuộc:

Character ACharacter\\ A

xuất hiện trên:

Character BCharacter\\ B

Ví dụ:

- scar đặc trưng của A xuất hiện trên B;

- hairstyle defining A xuất hiện trên B;

- face của B trở nên giống A;

- distinctive identity mark bị copy sang character khác.

AI-03 hiện đã định nghĩa Identity Leakage là một consistency validation category chính.

# **19. Leakage Severity**

QA có thể phân loại:

### **Minor**

Một attribute phụ có dấu hiệu leakage nhưng character vẫn phân biệt rõ.

### **Major**

Nhiều identity traits bị truyền.

### **Severe**

Hai character bị nhầm, merge hoặc gần như cùng identity.

Severity này là QA classification.

Không thay thế scientific metric của RESEARCH-04.

# **20. Character Swap**

Một failure riêng cần theo dõi:

Expected:

A on left

B on right

nhưng generated result:

A's identity assigned to B role

B's identity assigned to A role

Đây là:

> Character Binding Failure / Identity Swap.

Có thể được ghi như subtype của separation/leakage.

# **21. Appearance-State Alignment**

AI-03 yêu cầu kiểm tra appearance có phù hợp với scene state hay không.

Ví dụ canonical state:

Aria

outfit = black coat

left arm = injured/bandaged

hair = wet

Generated image cần phản ánh các state quan trọng đó.

# **22. Appearance State Test Cases**

### **AP-01 — Outfit Change**

Scene trước:

white shirt

Scene hiện tại:

black winter coat

Expected:

> identity giữ nguyên, outfit đổi.

### **AP-02 — Injury**

Canonical state:

left arm injured

Expected:

> visible state phù hợp khi scene yêu cầu thể hiện.

### **AP-03 — Equipment**

Character đã mất sword ở scene trước.

Scene sau không được tự động có sword trở lại nếu không có Event trả lại.

Domain specification đã dùng chính ví dụ này để mô tả consistency expectation.

# **23. Identity vs Appearance False Positive**

QA phải có test để chắc rằng hệ thống không warning sai.

Ví dụ:

same character

new outfit

Expected:

> không tự động coi là identity drift.

Nguồn domain cũng ghi rõ outfit khác scene trước có thể chỉ là WARNING vì user có thể chủ động đổi outfit.

# **24. Appearance Over-Constraint Test**

Scene yêu cầu outfit mới.

Model vẫn cố giữ outfit trong canonical reference.

Kết quả:

Identity = good

Appearance state = wrong

Đây không được xem là complete success.

# **25. Appearance Under-Constraint Test**

Scene variation mạnh làm character không còn recognizable.

Kết quả:

Appearance state = correct

Identity = wrong

Cũng là failure.

# **26. Scene Continuity**

Scene Continuity kiểm tra các dữ liệu cần giữ xuyên scene.

Ví dụ:

Scene 5:

A loses sword

Scene 8:

A still carries sword

không có Event trung gian.

Expected:

> consistency issue.

# **27. Continuity Areas**

QA có thể kiểm tra:

- possession;

- injury;

- location;

- relationship-dependent visual context;

- permanent visual change;

- character status;

- scene-specific item.

Narrative consistency chi tiết vẫn thuộc AI-02.

QA-02 tập trung phần nào ảnh hưởng generated image.

# **28. Scene Intent Alignment**

AI-03 quy định generation không được thay đổi hành động hoặc sự kiện cốt lõi của scene chỉ để model dễ sinh hơn.

Ví dụ Scene:

> Aria confronts Kael at the station.

Không acceptable nếu generation biến thành:

> Aria đứng một mình ở phòng ngủ.

dù identity đẹp.

# **29. Scene Intent Evaluation**

Kiểm tra:

- đúng active character;

- đúng interaction;

- đúng major action;

- đúng environment quan trọng;

- đúng emotional intent khi relevant.

Không yêu cầu pixel-level exact match.

# **30. Missing Character**

Nếu Scene có:

A + B

nhưng output chỉ có A:

> Semantic / Scene Alignment Failure.

Nếu multi-character generation thường xuyên mất secondary character:

> QA phải ghi pattern riêng.

# **31. Extra Character**

Nếu Scene chỉ có A nhưng output tạo thêm một character lạ:

> Scene Alignment Failure.

Severity phụ thuộc mức ảnh hưởng.

# **32. Visual / Image Quality**

QA-02 cũng phải kiểm tra basic image usability.

Các loại lỗi:

- corrupted image;

- malformed anatomy nghiêm trọng;

- unusable composition;

- broken face;

- duplicated body;

- severe artifacts.

Những lỗi này không nhất thiết là character consistency failure nhưng ảnh hưởng Candidate usability.

# **33. Technical Invalid Image**

Nếu image:

- không tồn tại;

- không đọc được;

- format không hỗ trợ;

- output bị corrupt;

thì phải được xử lý trước consistency evaluation.

AI-03 hiện quy định Technical Validation chạy trước các lớp semantic và consistency.

# **34. Evaluation Order**

QA evaluation nên theo thứ tự:

Technical Usability

↓

Character Presence

↓

Identity

↓

Multi-Character Separation

↓

Appearance State

↓

Scene Continuity

↓

Scene Intent

↓

Visual Quality

Nếu Technical Validation đã fail blocking:

> không cần diễn giải sâu character similarity của một file corrupt.

# **35. Validation Severity**

QA sử dụng severity tương thích AI-03:

### **INFO**

Không ảnh hưởng usability.

### **WARNING**

Output có khả năng sử dụng được nhưng có dấu hiệu lệch.

### **ERROR**

Output không đáp ứng requirement quan trọng.

### **BLOCKING**

Output tuyệt đối không được trở thành accepted output.

Đây là severity đã được AI-03 định nghĩa.

# **36. Warning Philosophy**

Warning không có nghĩa:

> candidate unusable.

Product requirement hiện xác định warning không tự động ngăn user xem candidate trừ khi output thuộc loại không được phép sử dụng.

Do đó QA phải kiểm tra:

WARNING

→ visible

→ explainable

→ user can review

# **37. Blocking Philosophy**

BLOCKING nghĩa:

> candidate không được trở thành accepted/selected output.

Ví dụ:

- image invalid;

- required Character identity hoàn toàn sai trong một hard rule;

- generated asset không tồn tại;

- output vi phạm một blocking policy.

UX phải phân biệt rõ blocking với warning.

# **38. Candidate Decision**

QA phải kiểm tra đúng bốn decision của AI-03:

ACCEPT

REVIEW_RECOMMENDED

REGENERATE_RECOMMENDED

REJECT

# **39. ACCEPT**

Expected khi:

- technical valid;

- required scene content đạt;

- không có consistency issue quan trọng;

- không có blocking condition.

User có thể select Candidate.

# **40. REVIEW_RECOMMENDED**

Expected khi:

> output usable nhưng có warning.

Ví dụ:

- hairstyle có dấu hiệu drift nhẹ;

- outfit khác nhưng có thể intentional;

- style slightly inconsistent.

User vẫn có quyền quyết định.

# **41. REGENERATE_RECOMMENDED**

Expected khi:

> output không đạt quality expectation nhưng không phải technical/system failure.

Ví dụ:

- identity sai đáng kể;

- secondary character biến dạng;

- scene state thiếu;

- leakage mạnh.

AI-03 định nghĩa đây là trường hợp output không đạt quality threshold nhưng không có system failure nghiêm trọng.

# **42. REJECT**

Expected khi:

> Candidate tuyệt đối không được sử dụng làm accepted output.

User có thể thấy error state/history tùy UX, nhưng không được select như valid result.

# **43. Validation Result Structure**

QA phải kiểm tra Candidate lưu được validation result tương thích cấu trúc:

technical

contract

semantic

consistency

overallDecision

Nguồn AI-03 minh họa consistency có:

status

score

findings

và overall decision có thể là REVIEW_RECOMMENDED.

# **44. Score Semantics**

Nguồn hiện tại cho phép validation result có score nhưng **chưa định nghĩa threshold chính thức**.

Do đó QA-02 không tự đặt:

0.8 = PASS

0.7 = WARNING

nếu AI implementation chưa khóa.

Các threshold cần được:

1.  calibration;

2.  document;

3.  version;

4.  freeze trước release evaluation.

# **45. Threshold Registry**

Khi threshold được quyết định, nên lưu dưới một configuration version.

Ví dụ:

ValidationProfile:

OWNIVERSE-QA-V1

chứa:

identityWarningThreshold

identityErrorThreshold

sceneAlignmentThreshold

leakageThreshold

Tên cụ thể phụ thuộc implementation.

# **46. No Hidden Threshold Change**

Không được thay threshold trực tiếp trên production/demo environment mà không thay validation profile/version.

Nếu:

QA-V1 → QA-V2

test result cần ghi rõ profile nào được dùng.

# **47. Automatic Evaluation vs Human Evaluation**

QA-02 sử dụng hai loại đánh giá.

### **Automatic**

Phù hợp với:

- technical validity;

- image similarity;

- reference similarity;

- character count;

- metadata;

- deterministic state checks.

### **Human**

Phù hợp với:

- subtle identity drift;

- leakage;

- visual quality;

- scene intent;

- difficult appearance changes.

# **48. Human Review Role**

Human review không được biến thành:

> “Tôi thấy đẹp nên PASS.”

Reviewer phải dựa vào evaluation criteria.

Ví dụ:

Identity

Appearance

Scene Intent

Continuity

Quality

được đánh giá riêng.

# **49. QA Human Rating**

Một simple operational scale có thể dùng trong QA:

PASS

PASS_WITH_WARNING

FAIL

BLOCKING

Đây là QA review label.

Nó không thay thế Candidate Decision trong application.

# **50. Human Review Evidence**

Khi reviewer đánh dấu FAIL cần ghi:

category

severity

character_id

scene_id

description

evidence

Ví dụ:

category:

IDENTITY_LEAKAGE

severity:

ERROR

description:

Kael inherits Aria's facial scar.

# **51. Standard QA Fixture**

Nên xây một reusable Story Fixture phục vụ regression.

Ví dụ:

Story QA-01

Character A — Aria

Character B — Kael

Scene 01 → Aria

Scene 02 → Aria + Kael

Scene 03 → Kael

Scene 04 → no Aria

...

Scene 10 → Aria returns

Fixture cần có canonical references rõ ràng.

# **52. Fixture Requirements**

Fixture nên bao phủ:

- single Character;

- two Characters;

- long reappearance;

- outfit change;

- injury;

- possession change;

- same-scene interaction.

Một fixture tốt có thể test nhiều consistency rule cùng lúc.

# **53. QA Case Group A — Identity**

QA2-ID-01

Same Character / Same Appearance

QA2-ID-02

Same Character / Different Pose

QA2-ID-03

Same Character / Different Camera

QA2-ID-04

Same Character / Different Lighting

Expected:

> identity vẫn recognizable.

# **54. QA Case Group B — Long Range**

QA2-LR-01

Character absent for short interval

QA2-LR-02

Character absent for medium interval

QA2-LR-03

Character absent for long interval

Mỗi case ghi raw reappearance gap.

# **55. QA Case Group C — Multi-Character**

QA2-MC-01

A + B same scene

QA2-MC-02

A + B interacting

QA2-MC-03

A + B different appearance states

Kiểm tra:

- presence;

- identity;

- separation;

- leakage.

# **56. QA Case Group D — Appearance**

QA2-AP-01

Outfit change

QA2-AP-02

Temporary injury

QA2-AP-03

Expression change

QA2-AP-04

Wet / weather state

QA2-AP-05

Equipment change

Expected:

> mutable state thay đổi, identity không bị mất.

# **57. QA Case Group E — Continuity**

QA2-CT-01

Lost item remains lost

QA2-CT-02

Permanent injury persists

QA2-CT-03

Location state respected

QA2-CT-04

Explicit state reset works

# **58. QA Case Group F — Scene Intent**

QA2-SC-01

Correct active characters

QA2-SC-02

Correct main action

QA2-SC-03

Correct environment

QA2-SC-04

Correct emotional intent

# **59. QA Case Group G — Invalid Output**

QA2-INV-01

Missing image

QA2-INV-02

Unsupported format

QA2-INV-03

Corrupted asset

QA2-INV-04

Model response without usable candidate

Expected:

> technical validation prevents normal acceptance.

# **60. QA Case Group H — Warning Behavior**

Test:

Candidate has WARNING

Expected:

- warning visible;

- candidate reviewable;

- action available;

- user có thể Keep/Select nếu không blocking.

Product workflow hiện quy định non-blocking warning vẫn cho phép user review và lựa chọn Candidate.

# **61. QA Case Group I — Blocking Behavior**

Test:

Candidate has BLOCKING finding

Expected:

- không select được;

- warning UI khác blocking UI;

- retry/regenerate action hiển thị phù hợp.

# **62. Auto-Regeneration Evaluation**

AI-03 cho phép validation failure tạo:

INVALID

hoặc:

REGENERATE_RECOMMENDED

sau đó application có thể generate attempt mới nếu policy cho phép.

QA phải kiểm tra:

- attempt count tăng;

- old Candidate vẫn giữ history;

- validation chạy lại;

- regeneration có giới hạn.

# **63. Retry vs Regeneration**

QA-02 đặc biệt không được nhầm:

### **Retry**

Technical/transient failure.

### **Regeneration**

Quality/creative failure.

AI-03 xác định retry dành cho timeout, network failure, rate limit, malformed response hoặc temporary inference failure.

Identity inconsistency không nên được xử lý như network retry.

# **64. Regeneration Success Test**

Scenario:

Attempt 1

→ Identity Consistency ERROR

→ REGENERATE_RECOMMENDED

Attempt 2

→ PASS

→ ACCEPT

Expected:

- cả hai attempt được giữ;

- Candidate 1 không biến mất;

- Candidate 2 liên kết đúng Job/Attempt;

- final decision phản ánh Candidate 2.

# **65. Maximum Generation Attempts**

Exact maximum attempt count thuộc operation policy/API/runtime config.

QA chỉ yêu cầu:

> không được infinite regenerate.

Test cần xác nhận hệ thống dừng theo configured limit.

# **66. Outdated Context Evaluation**

Scenario:

Job snapshot uses Character v3

↓

User edits Character → v4

↓

Job completes with v3

QA consistency evaluation của Candidate phải dùng:

> snapshot v3

để đánh giá generation đúng với input mà model thực sự nhận.

Sau đó application có thể đánh dấu candidate stale so với v4.

Không được chấm Candidate “sai” bằng data mà nó chưa từng nhận.

# **67. Stale vs Inconsistent**

Hai khái niệm phải tách.

### **Inconsistent**

Output không phù hợp với context snapshot dùng để generation.

### **Stale**

Output có thể hoàn toàn đúng với snapshot cũ nhưng canonical data đã đổi sau đó.

Đây là hai loại issue khác nhau.

# **68. Style Consistency**

Consistency Check trong product concept cũng bao gồm style deviation.

QA-02 có thể kiểm tra:

- expected project style;

- gross style deviation;

- visual coherence giữa scene.

Tuy nhiên style consistency không phải scientific core của thesis.

# **69. Style QA Case**

Ví dụ Project:

Manga B&W

Generated image:

photorealistic full-color image

Expected:

> style alignment failure.

Severity phụ thuộc product configuration.

# **70. Approved Image as Future Context**

Tài liệu sản phẩm ban đầu định hướng approved image có thể trở thành visual history cho generation tiếp theo.

Nếu feature này được triển khai, QA phải kiểm tra:

- chỉ approved/eligible output được dùng;

- rejected output không tự trở thành reference;

- provenance giữ được source Candidate.

Nếu MVP cuối không triển khai dynamic visual history thì các test này có thể NOT_APPLICABLE.

# **71. False Positive Testing**

Validator không chỉ cần phát hiện lỗi.

Nó cũng phải **không cảnh báo sai quá nhiều**.

Các legitimate changes cần test:

- outfit changed intentionally;

- expression changed;

- pose changed;

- camera changed;

- lighting changed;

- temporary appearance changed.

Nếu mọi variation đều WARNING:

> validator không hữu ích.

# **72. False Negative Testing**

Ngược lại cần test failure rõ:

- wrong face;

- missing character;

- swapped character;

- major identity leakage;

- missing injury;

- wrong critical item.

Nếu validator vẫn ACCEPT:

> false negative.

# **73. Validator QA Metrics**

Đối với validation mechanism, có thể theo dõi:

True Positive

False Positive

True Negative

False Negative

khi có manually labeled QA set.

Từ đó có thể tính các metric classification phù hợp.

Exact acceptance threshold:

> phải được calibration và version trước khi trở thành release gate.

# **74. QA Gold Set**

Nên xây một nhỏ QA Gold Set.

Nó chứa:

- known correct candidates;

- known warning candidates;

- known error candidates;

- known blocking candidates.

Mỗi sample có expected label.

Dùng để regression validator khi model/metric thay đổi.

# **75. Gold Set Example**

Gold-001

Correct Aria

Expected: ACCEPT

Gold-002

Aria slight hairstyle deviation

Expected: REVIEW_RECOMMENDED

Gold-003

Aria replaced by different identity

Expected: REGENERATE_RECOMMENDED

Gold-004

Corrupted image

Expected: REJECT

Các expected label phải được human-reviewed trước khi khóa Gold Set.

# **76. Model Change Regression**

Khi thay:

- diffusion model;

- image adapter;

- identity encoder;

- validation model;

- threshold profile;

phải chạy lại QA Gold Set.

Không nên assume model mới luôn tốt hơn.

# **77. Validation Profile Version**

Mỗi QA run nên ghi:

generationMethodVersion

validationMethodVersion

validationProfileVersion

để tránh so kết quả được đánh giá bằng hai validator khác nhau mà không biết.

# **78. Generation Metadata**

QA result nên liên kết với metadata gồm:

- recurring Character IDs;

- Character count;

- Scene position;

- distance from previous appearance;

- generation attempt;

- consistency validation result.

Đây là metadata mà AI-03 hiện đã dự kiến phục vụ research và analysis.

# **79. QA Evaluation Record**

Suggested logical record:

evaluationId

candidateId

jobId

attemptId

storyId

sceneId

characterIds

generationMethodVersion

validationProfileVersion

identityResult

longRangeResult

separationResult

leakageResult

appearanceResult

continuityResult

sceneAlignmentResult

imageQualityResult

overallQaResult

reviewer

evaluatedAt

notes

Đây là QA-layer record; exact physical schema thuộc DATA implementation nếu cần lưu persistent.

# **80. QA Result**

QA-level result có thể dùng:

PASS

PASS_WITH_WARNING

FAIL

BLOCKED

Ý nghĩa:

### **PASS**

Output và validator behavior đạt kỳ vọng.

### **PASS_WITH_WARNING**

Output vẫn usable nhưng có deviation hợp lệ được hệ thống thể hiện đúng.

### **FAIL**

Output hoặc validator không đáp ứng QA expectation.

### **BLOCKED**

Không thể đánh giá vì dependency/test environment có vấn đề.

# **81. AI-03 Decision vs QA Result**

Ví dụ:

AI-03 trả:

REVIEW_RECOMMENDED

và expected QA cũng là warning.

Khi đó:

QA Result = PASS

vì validator đã xử lý đúng.

QA không chấm FAIL chỉ vì Candidate bản thân có warning.

# **82. Example 1**

Candidate:

Aria correct identity

outfit correct

scene correct

AI-03:

ACCEPT

QA expected:

ACCEPT

Result:

PASS

# **83. Example 2**

Candidate:

Aria slightly different hairstyle

identity still recognizable

AI-03:

REVIEW_RECOMMENDED

QA expected:

REVIEW_RECOMMENDED

Result:

PASS

# **84. Example 3**

Candidate:

Kael has Aria's face

AI-03:

ACCEPT

Expected:

REGENERATE_RECOMMENDED

QA result:

FAIL

Đây là validator false negative.

# **85. Example 4**

Candidate:

Aria intentionally changed outfit

identity unchanged

AI-03:

REGENERATE_RECOMMENDED

Expected:

ACCEPT or REVIEW_RECOMMENDED

tùy scene specification.

QA:

FAIL

Đây có thể là validator false positive.

# **86. Batch Consistency Evaluation**

Large consistency evaluation có thể chạy asynchronous theo architecture hiện tại.

QA batch có thể dùng để:

- scan một story dài;

- kiểm tra recurring characters;

- tổng hợp warnings;

- tìm long-range drift.

# **87. Story-Level Consistency Summary**

Story QA report có thể bao gồm:

Total scenes

Generated scenes

Recurring characters

Warnings

Errors

Blocking candidates

Long-range issues

Leakage incidents

# **88. Character-Level Summary**

Ví dụ:

Character: Aria

Appearances: 12

Long-range reappearances: 3

Identity warnings: 1

Leakage cases: 0

Appearance-state errors: 1

Mục tiêu là dễ xác định Character nào có consistency kém.

# **89. Scene-Level Summary**

Ví dụ:

Scene 20

Characters:

Aria

Kael

Identity:

PASS / PASS

Separation:

WARNING

Appearance:

PASS

Scene:

PASS

Overall:

REVIEW_RECOMMENDED

# **90. Regression Trigger**

QA-02 regression nên được chạy khi thay đổi:

- image generation model;

- Character Reference logic;

- Context Builder;

- SceneCharacter state;

- prompt construction;

- consistency validator;

- identity metric;

- visual state resolution;

- AI adapter.

# **91. Minimum Release Gate**

Trước release/demo, tối thiểu:

- technical invalid cases được reject đúng;

- blocking Candidate không select được;

- warnings được hiển thị đúng;

- Character identity basic cases đạt;

- multi-character case hoạt động;

- long-range recurrence case được kiểm thử;

- appearance change không bị hiểu sai hoàn toàn;

- regeneration flow hoạt động.

# **92. Product Quality Threshold**

QA-02 v1 **không tự đặt numeric threshold** cho:

- identity similarity;

- leakage;

- scene alignment;

- image quality.

Các threshold chỉ nên được khóa sau khi:

1.  validator implementation tồn tại;

2.  QA Gold Set tồn tại;

3.  calibration được thực hiện;

4.  human review xác nhận threshold có ý nghĩa.

# **93. Threshold Freeze**

Trước thesis demo / release candidate phải tạo:

QA_VALIDATION_PROFILE_V1

với:

- metric implementation;

- thresholds;

- expected decision mapping;

- model/version.

Sau đó chạy lại QA Gold Set.

# **94. Acceptance Criteria — Identity**

### **QA2-AC-01**

Recurring Character SHOULD giữ recognizable identity qua nhiều Scene.

### **QA2-AC-02**

Legitimate Appearance change SHALL NOT tự động được coi là identity replacement.

### **QA2-AC-03**

Severe identity replacement SHALL NOT được ACCEPT.

# **95. Acceptance Criteria — Multi-Character**

### **QA2-AC-04**

Mỗi active Character SHOULD giữ identity riêng biệt.

### **QA2-AC-05**

Major identity leakage SHOULD dẫn tới warning/error phù hợp.

### **QA2-AC-06**

Identity swap SHALL không được ACCEPT như output hoàn toàn hợp lệ.

# **96. Acceptance Criteria — Appearance**

### **QA2-AC-07**

Critical visual state của Scene SHOULD được phản ánh trong Candidate.

### **QA2-AC-08**

Scene-specific appearance change SHALL được phép mà không thay đổi core identity.

# **97. Acceptance Criteria — Continuity**

### **QA2-AC-09**

Canonical state relevant tới image SHOULD được duy trì xuyên Scene.

### **QA2-AC-10**

Explicit canonical change SHALL override state cũ ở Scene sau thời điểm thay đổi.

# **98. Acceptance Criteria — Long Range**

### **QA2-AC-11**

QA suite SHALL có ít nhất một recurring-character reappearance case với khoảng Scene đáng kể.

### **QA2-AC-12**

Long-range QA result SHALL lưu raw reappearance distance.

# **99. Acceptance Criteria — Validation UX**

### **QA2-AC-13**

WARNING và BLOCKING SHALL được phân biệt.

### **QA2-AC-14**

Non-blocking warning SHALL vẫn cho phép review Candidate.

### **QA2-AC-15**

Blocking Candidate SHALL không thể trở thành selected output.

## **Những behavior này phù hợp Product/UX hiện tại.**

# **100. Acceptance Criteria — Provenance**

### **QA2-AC-16**

QA phải truy được Candidate tới:

Job

Attempt

Context Snapshot

Generation Method

Validation Result

### **QA2-AC-17**

Generation history SHALL không mất khi regenerate.

# **101. Out of Scope**

QA-02 không định nghĩa:

- SAMIM neural architecture;

- embedding model cụ thể;

- diffusion attention algorithm;

- training loss;

- statistical proof của thesis;

- final research benchmark methodology;

- human-study statistical analysis.

Những nội dung đó thuộc RESEARCH-03/04.

# **102. Required QA Artifacts**

QA-02 nên tạo ra:

QA Gold Set

Consistency Test Fixture

AI Validation Test Cases

Image Evaluation Records

Long-Range QA Report

Multi-Character QA Report

Validator Regression Report

Release Validation Profile

# **103. Final Principle**

Nguyên tắc cốt lõi của QA-02:

> **Consistency không có nghĩa là mọi ảnh phải giống nhau. Consistency nghĩa là những gì phải ổn định thì ổn định, và những gì được phép thay đổi thì thay đổi đúng với trạng thái của câu chuyện.**

Do đó một Candidate tốt phải đồng thời đáp ứng:

Correct Identity

\+

Correct Scene Appearance

\+

Correct Character Separation

\+

Correct Continuity

\+

Correct Scene Intent

\+

Usable Image Quality

QA-02 biến yêu cầu trừu tượng:

> **“Giữ nhân vật nhất quán trong truyện dài”**

thành một tập evaluation case, severity, decision và acceptance criteria mà OWNIVERSE có thể kiểm thử lặp lại trong quá trình phát triển.
