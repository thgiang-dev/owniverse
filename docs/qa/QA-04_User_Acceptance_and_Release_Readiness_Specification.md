# **QA-04 — User Acceptance & Release Readiness Specification**

**Document ID:** QA-04  
**Document Type:** User Acceptance & Release Readiness Specification  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Status:** Draft v1  
**Primary Roles:** Product Owner / QA Engineer / Thesis Author / UAT Participant  
**Parent Document:** QA-01 — Master Test Plan & Quality Strategy  
**Related Documents:** QA-02, QA-03, PROD-01, PROD-02, UX-01, UX-02, API-01, API-02, ARCH-02, DEPLOY-01, DEPLOY-02, DEPLOY-03

# **1. Purpose**

QA-04 định nghĩa tiêu chí cuối cùng để xác định OWNIVERSE có đủ điều kiện:

- được người dùng thử nghiệm;

- được xem là đạt MVP;

- trở thành Release Candidate;

- chạy trong Thesis Demo Environment;

- sử dụng trong buổi bảo vệ luận văn.

QA-04 không kiểm tra từng API hoặc business rule riêng lẻ.

Những phần đó đã thuộc QA-03.

Tài liệu tập trung vào câu hỏi:

> **Người dùng có thực sự hoàn thành được mục tiêu chính của OWNIVERSE bằng một workflow hoàn chỉnh hay không?**

và:

> **Build hiện tại có đủ ổn định để được sử dụng trong demo/bảo vệ hay chưa?**

# **2. Position in QA Process**

Luồng QA tổng quát:

QA-01

Quality Strategy

↓

QA-02

AI / Consistency Evaluation

↓

QA-03

Functional / Integration Testing

↓

QA-04

User Acceptance / Release Readiness

QA-04 là quality gate cuối của application trước deployment/demo.

# **3. User Acceptance Testing**

Tài liệu Testing ban đầu đã xác định **User Acceptance Test** là bước để người dùng trực tiếp thử sản phẩm.

UAT không hỏi:

> API endpoint có trả đúng status code không?

UAT hỏi:

> Người dùng có thể hoàn thành công việc thực tế của họ không?

# **4. UAT Objective**

UAT phải xác nhận rằng một creator có thể:

1.  bắt đầu từ story idea;

2.  tạo Project;

3.  xây Story;

4.  tạo recurring Characters;

5.  quản lý Character Reference;

6.  tạo Scene sequence;

7.  gán Character vào Scene;

8.  generate image;

9.  review Candidate;

10. chọn Candidate;

11. regenerate khi cần;

12. tiếp tục story mà không mất history;

13. hoàn thành một story sequence trong cùng hệ thống.

Các mục tiêu này tương ứng trực tiếp với Product Success Criteria SC-01 đến SC-09.

# **5. UAT Is Not Research Evaluation**

UAT không dùng để chứng minh:

> SAMIM tốt hơn StoryDiffusion.

Đó là nhiệm vụ của RESEARCH-04.

UAT chỉ kiểm tra:

> người dùng có thể sử dụng feature character consistency trong một workflow thực tế hay không.

# **6. UAT Participants**

Đối với luận văn MVP, participant có thể gồm:

- thesis author;

- bạn học;

- developer không trực tiếp code module đó;

- người có quan tâm tới manga/story creation;

- supervisor nếu phù hợp.

Không bắt buộc phải có user study quy mô lớn nếu thesis không đặt mục tiêu nghiên cứu HCI.

# **7. Minimum UAT Participant Principle**

QA-04 không cố định số người tham gia từ tài liệu nguồn.

Quy mô UAT phải phù hợp mục tiêu:

> **functional product acceptance**, không phải statistical human study.

Nếu cần nghiên cứu perception có statistical analysis thì thuộc RESEARCH-04.

# **8. UAT Environment**

UAT nên chạy trên:

Demo / Thesis Environment

không nên chạy trên environment developer đang chỉnh code trực tiếp.

ARCH-02 hiện xác định ba environment logic:

- Development;

- Demo / Thesis;

- Production-like.

# **9. Thesis Demo Environment Requirements**

Demo / Thesis Environment phải hướng tới:

- ổn định;

- reproducible;

- đủ mạnh để demo end-to-end.

QA-04 coi ba đặc điểm này là release requirement.

# **10. UAT Entry Criteria**

UAT chỉ bắt đầu khi:

### **UC-ENTRY-01**

QA-03 P0 tests đã PASS.

### **UC-ENTRY-02**

Không còn blocker làm hỏng core user journey.

### **UC-ENTRY-03**

Database migration của build đã được áp dụng.

### **UC-ENTRY-04**

Required services đang healthy.

### **UC-ENTRY-05**

AI generation ít nhất hoạt động bằng real provider hoặc demo-ready adapter theo scope UAT.

### **UC-ENTRY-06**

Standard UAT data đã sẵn sàng.

# **11. UAT Test Data**

Nên chuẩn bị một UAT Project template gồm:

Story Idea

Characters:

Aria

Kael

Canonical References

Several Scenes

One recurring-character reappearance

One multi-character Scene

One appearance change

Participant có thể:

- dùng template;

- hoặc tạo Project mới.

Core UAT nên có ít nhất một workflow từ dữ liệu mới để chứng minh hệ thống không chỉ hoạt động với prepared demo data.

# **12. Product Success Mapping**

QA-04 sử dụng Product Success Criteria làm acceptance backbone.

### **SC-01**

User tạo được structured Project từ Story Idea.

### **SC-02**

User tạo được nhiều recurring Characters có identity riêng.

### **SC-03**

User tạo được Story có nhiều Scene.

### **SC-04**

User tạo được image mà không cần tự xây technical prompt đầy đủ.

### **SC-05**

Character tái xuất hiện ở Scene xa hơn vẫn dùng đúng Character Context.

### **SC-06**

User review nhiều Candidate và chọn output mong muốn.

### **SC-07**

Regenerate không mất output cũ.

### **SC-08**

Canonical data mới được sử dụng cho generation tiếp theo.

### **SC-09**

User hoàn thành được Story sequence trong cùng application workflow.

# **13. Critical User Journeys**

QA-04 xác định năm Critical User Journeys.

CUJ-01

Create Story Project

CUJ-02

Build Character & Scene Context

CUJ-03

Generate & Select Image

CUJ-04

Regenerate & Preserve History

CUJ-05

Long-Range Story Continuation

Tất cả phải hoạt động để MVP đạt acceptance.

# **14. CUJ-01 — Create Story Project**

## **Goal**

User bắt đầu từ ý tưởng và tạo được Project có Story.

### **Steps**

1.  Mở OWNIVERSE.

2.  Create Project.

3.  Nhập Story Idea.

4.  Tạo hoặc xác nhận Story.

5.  Mở Story workspace.

### **Expected Outcome**

- Project tồn tại;

- Story tồn tại;

- user hiểu mình đang ở Project nào;

- user có thể tiếp tục Character/Scene workflow.

# **15. CUJ-01 Acceptance**

### **UAT-CUJ01-01**

Participant hoàn thành Project creation mà không cần developer sửa database/manual intervention.

### **UAT-CUJ01-02**

Failure không tạo Project dở dang trong normal Project list.

Điều này phù hợp acceptance criteria hiện có của WF-01.

# **16. CUJ-02 — Build Character & Scene Context**

## **Goal**

User tạo Characters và cấu trúc Scene cần thiết cho generation.

### **Steps**

1.  Create Aria.

2.  Edit Character identity.

3.  Upload Reference.

4.  Set canonical Reference.

5.  Create multiple Scenes.

6.  Assign Characters.

7.  Define current visual state.

### **Expected Outcome**

User có thể hiểu rõ:

Who the Character is

khác với:

How the Character looks in this Scene

# **17. CUJ-02 Acceptance**

Participant phải có thể:

- tạo nhiều Character;

- chỉnh Character;

- upload Reference;

- xác định canonical reference;

- tạo Scene;

- reorder Scene;

- xác định active Characters.

Các workflow này đều thuộc nhóm P0 Must Have của MVP.

# **18. CUJ-03 — Generate & Select Image**

## **Goal**

User generate image dựa trên Scene Context.

### **Steps**

1.  Mở Scene.

2.  Nhấn Generate Image.

3.  Quan sát trạng thái processing.

4.  Candidate xuất hiện.

5.  Review Candidate.

6.  Xem warning nếu có.

7.  Select Candidate.

### **Expected Outcome**

User không cần hiểu:

- diffusion implementation;

- Python worker;

- prompt engineering chi tiết;

- queue internals.

Hệ thống chuyển story context thành generation workflow.

# **19. CUJ-03 Acceptance**

### **UAT-CUJ03-01**

User biết generation đang chạy.

### **UAT-CUJ03-02**

UI không bị khóa toàn bộ khi Job đang chạy.

### **UAT-CUJ03-03**

Candidate được hiển thị rõ.

### **UAT-CUJ03-04**

User phân biệt được Candidate selected và chưa selected.

### **UAT-CUJ03-05**

Warning và blocking state không bị nhầm.

# **20. Candidate Warning Acceptance**

Khi Candidate có warning:

User phải hiểu:

> output có thể có consistency issue nhưng vẫn review được.

Actions nên rõ như:

Review

Regenerate

Keep / Select where allowed

# **21. CUJ-04 — Regenerate & Preserve History**

## **Goal**

User thử output khác mà không mất output trước.

### **Steps**

1.  Scene đang có Candidate A.

2.  User Regenerate.

3.  Candidate B xuất hiện.

4.  User review B.

5.  User mở History.

6.  Xem lại A.

7.  Chọn A hoặc B.

### **Expected Outcome**

History chứa cả A và B.

# **22. CUJ-04 Acceptance**

### **UAT-CUJ04-01**

Regeneration không xóa Candidate trước.

### **UAT-CUJ04-02**

User biết Candidate nào đang selected.

### **UAT-CUJ04-03**

User xem lại Candidate cũ.

### **UAT-CUJ04-04**

Candidate hợp lệ trước đó có thể được re-select.

Regeneration không được xóa generation history là business rule cấp sản phẩm.

# **23. CUJ-05 — Long-Range Story Continuation**

## **Goal**

Đánh giá use case đặc trưng nhất của sản phẩm.

Scenario:

Scene 01 → Aria

Scene 02 → Kael

Scene 03 → Kael

...

Scene 10 → Aria returns

### **Expected Outcome**

Generation Scene 10 vẫn lấy đúng Character Context của Aria.

Product Success Criteria cũng yêu cầu Character tái xuất hiện ở Scene xa vẫn sử dụng đúng Character Context.

# **24. CUJ-05 Acceptance**

### **UAT-CUJ05-01**

System xác định đúng active Character.

### **UAT-CUJ05-02**

Correct Character Reference/identity context được dùng.

### **UAT-CUJ05-03**

User có thể review consistency information nếu feature hỗ trợ.

### **UAT-CUJ05-04**

Generation hoàn thành bình thường dù Character không xuất hiện ở Scene ngay trước.

# **25. Canonical Change Journey**

Một journey quan trọng khác:

Generate using Character v3

↓

Edit Character → v4

↓

Old Candidate remains

↓

New Generation uses v4

### **Expected Outcome**

User nhận biết:

- output cũ vẫn tồn tại;

- output cũ có thể outdated;

- generation mới dùng canonical state mới.

# **26. UAT Canonical Change Acceptance**

### **UAT-CAN-01**

Editing Character không xóa output cũ.

### **UAT-CAN-02**

User được thông báo Candidate nào có context cũ khi feature hỗ trợ detection.

### **UAT-CAN-03**

New generation dùng canonical state mới.

# **27. AI-Assisted Story Content Acceptance**

Nếu MVP triển khai WF-09:

AI-generated Story/Scene suggestion phải là:

> proposal

trước khi trở thành canonical.

Acceptance criteria hiện có yêu cầu user có thể review generated Scene content trước canonical update nếu workflow yêu cầu confirmation.

# **28. UAT Usability Dimensions**

Participant đánh giá theo năm dimension.

### **Task Completion**

Có làm xong được không?

### **Clarity**

Có hiểu bước tiếp theo không?

### **Control**

Có cảm thấy mình kiểm soát Story/AI không?

### **Feedback**

Có hiểu system đang generating/warning/failing không?

### **Recovery**

Khi output không đúng, có biết làm gì tiếp không?

# **29. UAT Rating**

Có thể dùng simple scale:

1 — Very Difficult

2 — Difficult

3 — Acceptable

4 — Easy

5 — Very Easy

cho các câu hỏi usability.

Exact quantitative acceptance threshold chưa được tài liệu nguồn định nghĩa.

Nếu sử dụng numeric threshold, nó phải được khóa trước UAT chính thức.

# **30. UAT Task Status**

Mỗi task có:

COMPLETED

COMPLETED_WITH_HELP

FAILED

BLOCKED

# **31. COMPLETED**

Participant hoàn thành task mà không cần developer intervention.

Clarification nhỏ về wording có thể không tính là help tùy protocol đã định nghĩa.

# **32. COMPLETED_WITH_HELP**

Participant hoàn thành nhưng cần:

- hướng dẫn vị trí feature;

- giải thích terminology;

- developer chỉ đường.

Nhiều task ở trạng thái này có thể cho thấy UX problem dù chức năng kỹ thuật PASS.

# **33. FAILED**

Participant không hoàn thành task do:

- UX confusion;

- feature bug;

- workflow thiếu;

- system behavior sai.

# **34. BLOCKED**

Không đánh giá được vì:

- environment down;

- AI provider unavailable;

- test data lỗi;

- infrastructure failure.

BLOCKED không được coi là PASS.

# **35. UAT Observation Record**

Mỗi task nên ghi:

Participant ID

Build Version

Environment

Task ID

Start

End

Completion Status

Observed Problems

User Comments

Required Help

Defect / Improvement Link

# **36. UAT Issue Classification**

Finding được phân thành:

DEFECT

USABILITY_ISSUE

CONTENT_ISSUE

AI_QUALITY_ISSUE

DOCUMENTATION_ISSUE

ENHANCEMENT

Không phải mọi complaint đều là functional defect.

# **37. UAT Blocker**

Ví dụ blocker:

- không tạo được Project;

- không tạo được Character;

- Generate Image không chạy;

- Candidate không xuất hiện;

- không select được valid Candidate;

- data mất sau refresh.

Những lỗi này trực tiếp phá Product Success Criteria.

# **38. UAT Major Issue**

Ví dụ:

- user không tìm được Character Reference workflow;

- warning khó hiểu;

- regeneration tạo output nhưng user không tìm lại được history;

- Scene reorder gây confusion.

Không nhất thiết crash nhưng ảnh hưởng mạnh core experience.

# **39. UAT Minor Issue**

Ví dụ:

- label chưa rõ;

- spacing;

- wording;

- non-critical navigation friction.

Có thể không block thesis demo.

# **40. MVP Acceptance Scope**

MVP phục vụ luận văn phải hỗ trợ tối thiểu các khả năng từ Project creation tới consistency validation và persistence.

Các feature future như:

- full manga editor;

- marketplace;

- mobile app;

- collaboration;

không được dùng làm release blocker cho MVP.

# **41. Scope Protection Rule**

Không được đánh dấu MVP là:

> NOT READY

chỉ vì feature Future Scope chưa tồn tại.

Acceptance phải dựa trên agreed MVP.

# **42. Workflow Priority**

Release readiness ưu tiên:

### **P0 Must Have**

- Create Project;

- Initialize Story;

- Create/Manage Character;

- Character Reference;

- Scene Management;

- Scene Character Assignment;

- Generate Image;

- Review Candidate;

- Select Candidate;

- Regenerate;

- Generation History.

### **P1 Important**

- Manage Story Core;

- AI-Assisted Scene Generation;

- Handle Canonical Changes.

# **43. P0 Release Rule**

Release Candidate không được xem là ready nếu một P0 workflow không thể hoàn thành trong normal happy path.

# **44. P1 Release Rule**

P1 issue có thể cho phép Conditional Go nếu:

- core P0 workflow unaffected;

- workaround tồn tại;

- limitation được document;

- không ảnh hưởng research demo cốt lõi.

# **45. P2 Release Rule**

P2 enhancement không block thesis demo nếu chưa hoàn thành.

# **46. Release Readiness Layers**

Release decision gồm sáu layers:

Functional

AI

Data

Infrastructure

UX/UAT

Demo Readiness

Một build phải được xem xét toàn diện.

# **47. Functional Readiness**

Yêu cầu:

- QA-03 P0 PASS;

- critical P1 không phá workflow;

- không có data corruption bug.

# **48. AI Readiness**

Yêu cầu:

- image generation hoạt động;

- validation pipeline hoạt động ở mức MVP;

- Candidate được trả đúng workflow;

- critical QA-02 scenarios được kiểm tra.

Không yêu cầu model phải “hoàn hảo”.

# **49. Data Readiness**

Yêu cầu:

- canonical data persist;

- selected Candidate persist;

- generation history persist;

- context/provenance cần cho luận văn được lưu;

- database schema/migration ổn định.

# **50. Infrastructure Readiness**

Yêu cầu:

Frontend reachable

Backend reachable

Database healthy

Queue healthy

Storage healthy

AI Worker reachable

nếu các component đó được dùng trong deployment topology.

# **51. UX Readiness**

Yêu cầu:

- user biết Project/Scene hiện tại;

- generation state rõ;

- selected Candidate rõ;

- warning khác blocking;

- recovery action rõ.

# **52. Demo Readiness**

Thesis Demo Environment được yêu cầu ổn định, reproducible và chạy được end-to-end.

Do đó trước demo:

> build phải được chạy thử trên chính topology dự định dùng khi bảo vệ.

Không chỉ chạy trên developer machine khác environment.

# **53. Release Decision**

QA-04 sử dụng ba quyết định:

GO

CONDITIONAL_GO

NO_GO

# **54. GO**

Build được GO khi:

- mọi P0 journey PASS;

- không có unresolved S0/S1;

- demo environment healthy;

- generation end-to-end PASS;

- data persistence PASS;

- known limitations không ảnh hưởng mục tiêu luận văn.

# **55. CONDITIONAL_GO**

Có thể sử dụng khi:

- P0 core vẫn hoạt động;

- còn issue P1/P2;

- workaround rõ;

- risk chấp nhận được;

- issue được document.

Ví dụ:

> SignalR reconnect đôi lúc chậm nhưng REST polling luôn chính xác.

Nếu không phá demo flow:

> Conditional Go có thể hợp lý.

# **56. NO_GO**

Build phải NO_GO khi:

- không tạo được Project/Story;

- Generate Image không hoạt động;

- Candidate mất;

- selected Candidate không persist;

- history mất khi regenerate;

- wrong Project access;

- database corruption;

- demo environment không thể khởi động ổn định.

# **57. Thesis Demo Critical Path**

Critical demo path:

Open OWNIVERSE

↓

Open/Create Project

↓

Show Story

↓

Show Characters

↓

Show Character Reference

↓

Open Scene

↓

Generate Image

↓

Observe Job Progress

↓

Review Candidate

↓

Select Candidate

↓

Show History

↓

Show Recurring Character in Later Scene

Nếu path này chạy được:

> sản phẩm thể hiện gần như toàn bộ contribution application của luận văn.

# **58. Demo Scenario A — Product Overview**

Mục tiêu:

> chứng minh OWNIVERSE là Story Creation Studio chứ không phải giao diện gọi image API.

Show:

- Project;

- Story;

- Character;

- Scene structure.

# **59. Demo Scenario B — Character Context**

Show:

Character Identity

Character Reference

Scene-specific Visual State

Giải thích:

> identity và appearance được tách.

# **60. Demo Scenario C — Generate Scene**

Show:

Generate Image

↓

Queued

↓

Running

↓

Validating

↓

Candidate

# **61. Demo Scenario D — Candidate Workflow**

Show:

- multiple Candidate;

- warning;

- select;

- regenerate;

- history.

Đây chứng minh AI không trực tiếp overwrite Canon.

# **62. Demo Scenario E — Long-Range Character**

Show:

Scene 01 → Aria

...

Scene 10 → Aria

và Character context/references vẫn được sử dụng.

Đây là bridge trực tiếp tới research problem.

# **63. Demo Live Generation vs Prepared Results**

Không nên phụ thuộc hoàn toàn vào live AI generation.

Nên có:

### **Live Path**

Để chứng minh pipeline hoạt động.

### **Prepared Candidate**

Để bảo đảm có material trình bày nếu GPU/provider chậm.

Prepared result phải là output thực của hệ thống, không giả tạo provenance.

# **64. Demo Fallback Principle**

Fallback không được che giấu rằng live generation thất bại.

Nếu cần dùng prepared output:

> trình bày rõ đây là pre-generated Candidate dùng cho demonstration continuity.

# **65. External Dependency Risk**

Các dependency có thể ảnh hưởng demo:

- remote GPU;

- Colab;

- network;

- AI API;

- queue;

- object storage.

ARCH-02 cho phép GPU worker trong thesis demo chạy trên Colab Pro hoặc environment riêng, nhưng cũng xác định Colab không phải production infrastructure lâu dài.

# **66. Demo Fallback Assets**

Nên chuẩn bị:

- previously generated Candidates;

- Character References;

- Story data;

- screenshots chỉ như backup presentation;

- database backup.

Không dùng screenshot thay toàn bộ live application nếu application có thể chạy.

# **67. Pre-Demo Smoke Test**

Ngay trước demo environment freeze, chạy:

Open frontend

Health backend

Health database

Health queue

Health storage

Health worker

Open Project

Open Scene

Generate test Candidate

Select Candidate

Reload

Check history

# **68. Demo Freeze**

Trước buổi bảo vệ nên tạo một build/tag:

THESIS_DEMO_RC_1

Sau freeze:

- không thêm feature không cần thiết;

- chỉ sửa blocker/critical issue;

- mọi sửa phải regression lại critical path.

# **69. Database Freeze Strategy**

Không có nghĩa database không được ghi.

Nó có nghĩa:

> schema/migration cho demo build không nên thay đổi tùy tiện sát buổi bảo vệ.

Mọi migration mới phải được test trên backup/clean environment.

# **70. Demo Configuration Freeze**

Freeze:

- application version;

- database migration version;

- worker version;

- model/method version;

- validation configuration;

- environment variables cần thiết.

# **71. Release Checklist — Application**

\[ \] Frontend build succeeds

\[ \] Backend build succeeds

\[ \] Database migration succeeds

\[ \] Core routes accessible

\[ \] Project opens

\[ \] Character opens

\[ \] Scene opens

\[ \] Candidate history opens

# **72. Release Checklist — Generation**

\[ \] Generation request accepted

\[ \] Job created

\[ \] Worker receives Job

\[ \] Image generated

\[ \] Asset saved

\[ \] Candidate created

\[ \] Validation completed

\[ \] Candidate visible

\[ \] Candidate selectable

# **73. Release Checklist — Data**

\[ \] Canonical data persists

\[ \] References persist

\[ \] Selected Candidate persists

\[ \] History persists

\[ \] Context snapshot exists

\[ \] Generation provenance exists

# **74. Release Checklist — Failure Handling**

\[ \] Missing generation data shows useful error

\[ \] Failed Job does not delete selected image

\[ \] Blocking Candidate cannot be selected

\[ \] Regeneration preserves history

\[ \] Browser refresh does not lose Job

# **75. Release Checklist — UX**

\[ \] Generation state understandable

\[ \] Warning understandable

\[ \] Blocking state understandable

\[ \] Selected Candidate obvious

\[ \] Regenerate action discoverable

\[ \] History discoverable

# **76. Release Checklist — Security**

\[ \] Project ownership checked

\[ \] Private asset access protected

\[ \] Secrets not exposed to frontend/repository

# **77. Release Checklist — Demo**

\[ \] Demo Project prepared

\[ \] Fresh Project creation tested

\[ \] Long-range example prepared

\[ \] Multi-character example prepared

\[ \] Backup database available

\[ \] Prepared Candidate available

\[ \] Demo build/tag recorded

\[ \] Environment startup instructions verified

# **78. Known Limitations**

Mọi limitation chưa sửa phải ghi:

ID

Description

Impact

Affected Workflow

Severity

Workaround

Demo Impact

Future Resolution

# **79. Limitation Acceptance**

Một known limitation chỉ được chấp nhận nếu:

- không phá core thesis objective;

- không gây data loss;

- không gây security issue nghiêm trọng;

- có workaround;

- được biết trước demo.

# **80. Example Acceptable Limitation**

> Mobile workspace chưa tối ưu hoàn chỉnh.

MVP đã ưu tiên desktop/web và advanced mobile creative workspace không bắt buộc.

Có thể không block thesis demo.

# **81. Example Unacceptable Limitation**

> Regeneration đôi lúc xóa history.

Điều này vi phạm business rule cốt lõi và Product Success Criteria.

Không nên GO.

# **82. Product Success Acceptance**

Release Candidate phải chứng minh:

SC-01 PASS

SC-02 PASS

SC-03 PASS

SC-04 PASS

SC-05 PASS

SC-06 PASS

SC-07 PASS

SC-08 PASS

SC-09 PASS

Nếu một SC chưa implement do scope thay đổi:

> Product Specification phải được cập nhật rõ, không âm thầm bỏ acceptance criterion.

# **83. UAT Summary Report**

Sau UAT tạo:

Build Version

Participants

Tasks Executed

Completed

Completed With Help

Failed

Blocked

Major Findings

Known Limitations

Recommended Release Decision

# **84. Release Readiness Report**

Report cuối:

Build

Environment

Date

QA-02 Status

QA-03 Status

UAT Status

Infrastructure Status

Open S0

Open S1

Open S2

Known Limitations

Decision:

GO / CONDITIONAL_GO / NO_GO

# **85. Release Evidence**

Nên giữ:

- CI/build result;

- test result;

- UAT notes;

- screenshots;

- Job IDs;

- Candidate examples;

- deployment version;

- commit/tag.

Điều này hỗ trợ cả QA và báo cáo luận văn phần Implementation/Evaluation.

# **86. Final Acceptance Gate**

OWNIVERSE MVP được xem là đủ điều kiện demo khi:

1.  user hoàn thành Critical User Journey;

2.  P0 workflows hoạt động;

3.  generation chạy end-to-end;

4.  Candidate workflow hoàn chỉnh;

5.  history và canonical data được bảo vệ;

6.  recurring Character workflow hoạt động;

7.  QA-02 không phát hiện blocker chưa xử lý;

8.  QA-03 critical test PASS;

9.  Demo / Thesis environment ổn định;

10. known limitations được document.

# **87. Final Principle**

Nguyên tắc cuối của QA-04:

> **Một sản phẩm không được xem là sẵn sàng chỉ vì từng module đều chạy; nó chỉ sẵn sàng khi người dùng có thể hoàn thành mục tiêu từ đầu đến cuối trên chính environment sẽ được sử dụng thực tế.**

Đối với OWNIVERSE, acceptance cuối cùng không phải:

> “API hoạt động.”

mà là:

> **“Một creator có thể bắt đầu từ story idea, xây Character và Scene, tạo image bằng AI, review/select/regenerate output, tiếp tục story dài và không mất quyền kiểm soát Canon hoặc generation history.”**

Đó là tiêu chuẩn cuối cùng để OWNIVERSE chuyển từ:

> **Implemented**

sang:

> **Demo Ready / Release Ready**.
