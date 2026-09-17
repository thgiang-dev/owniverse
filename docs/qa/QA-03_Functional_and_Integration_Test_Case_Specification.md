# **QA-03 — Functional & Integration Test Case Specification**

**Document ID:** QA-03  
**Document Type:** Functional & Integration Test Case Specification  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Status:** Draft v1  
**Primary Roles:** QA Engineer / Backend Developer / Frontend Developer  
**Parent Document:** QA-01 — Master Test Plan & Quality Strategy  
**Related Documents:** PROD-01, PROD-02, DOMAIN-01, DOMAIN-02, DATA-01, DATA-02, API-01, API-02, AI-01, AI-02, AI-03, QA-02, QA-04

# **1. Purpose**

QA-03 chuyển các Product Requirement, Workflow, Domain Rule, API Contract và Runtime Contract của OWNIVERSE thành các **test case có thể thực thi**.

Tài liệu tập trung vào câu hỏi:

> **Hệ thống đã hiện thực đúng các workflow được đặc tả hay chưa?**

QA-03 bao phủ các luồng MVP quan trọng:

- Project;

- Story;

- Character;

- Character Reference;

- Scene;

- Scene–Character assignment;

- API contract;

- optimistic concurrency;

- image generation;

- asynchronous Job;

- Candidate;

- Candidate selection;

- regeneration;

- generation history;

- stale/outdated context;

- retry;

- cancellation;

- persistence;

- authorization;

- storage integration.

Các resource chính này cũng chính là những entity mà API-01 hiện định nghĩa cho frontend/backend integration.

# **2. Scope**

QA-03 kiểm tra:

### **Functional Behavior**

User action có tạo đúng business result hay không.

### **Business Rules**

Các rule trong Product/Domain có được enforce hay không.

### **API Contract**

Frontend/backend có tuân thủ contract hay không.

### **Persistence**

Dữ liệu có được lưu và load chính xác hay không.

### **Integration**

Database, Queue, Worker, Storage và Backend có phối hợp đúng không.

### **Failure Behavior**

Khi một dependency lỗi, dữ liệu hiện tại có được bảo vệ không.

# **3. Out of Scope**

QA-03 không đánh giá:

- chất lượng artistic của generated image;

- identity similarity;

- long-range character consistency metric;

- SAMIM benchmark;

- research hypothesis.

Những phần đó thuộc:

> **QA-02** và **RESEARCH-04**.

# **4. Test Case Format**

Mỗi test case sử dụng format:

Test ID

Requirement / Workflow

Title

Priority

Test Type

Preconditions

Test Data

Steps

Expected Result

Automation Candidate

Evidence

# **5. Test ID Convention**

Test ID:

QA3-{AREA}-{NUMBER}

Ví dụ:

QA3-PROJ-001

QA3-CHAR-003

QA3-SCENE-005

QA3-GEN-001

QA3-JOB-004

# **6. Priority**

Sử dụng priority từ QA-01:

P0 — Critical

P1 — High

P2 — Medium

P3 — Low

P0 và P1 tạo regression suite chính.

# **7. Test Types**

QA-03 sử dụng:

FUNCTIONAL

API

INTEGRATION

DATABASE

ASYNC

AUTHORIZATION

RECOVERY

E2E

Một test case có thể thuộc nhiều loại.

# **8. Standard Test Data**

Nên có reusable test fixture:

User:

User-A

Project:

Project-Alpha

Story:

Story-Alpha

Characters:

Aria

Kael

References:

Aria-Ref-01

Kael-Ref-01

Scenes:

Scene-01

Scene-02

Scene-03

Trong đó:

Scene-01 → Aria

Scene-02 → Aria + Kael

Scene-03 → Kael

# **9. Test Execution Principle**

Automated tests phải:

Arrange

↓

Act

↓

Assert

↓

Cleanup

Không phụ thuộc test trước.

# **10. Test Group — Project**

## **QA3-PROJ-001 — Create Project**

**Priority:** P0  
**Type:** Functional / API / Database

### **Preconditions**

Authenticated User-A.

### **Steps**

1.  User chọn Create Project.

2.  Nhập tên Project hợp lệ.

3.  Submit.

### **Expected Result**

- Project được tạo;

- Project thuộc User-A;

- Project có unique ID;

- dữ liệu persist;

- reload vẫn thấy Project;

- API trả response success.

# **11. QA3-PROJ-002 — Load Existing Project**

**Priority:** P0  
**Type:** Functional / Database

### **Preconditions**

Project-Alpha tồn tại.

### **Steps**

1.  Reload application.

2.  Mở Project-Alpha.

### **Expected Result**

Project data load lại đúng.

Không mất:

- Story;

- Character;

- Scene;

- Candidate history.

# **12. QA3-PROJ-003 — Unauthorized Project Access**

**Priority:** P0  
**Type:** Authorization / API

### **Preconditions**

Project thuộc User-A.

### **Steps**

1.  Login User-B.

2.  Gọi endpoint Project-A bằng ID trực tiếp.

### **Expected Result**

- Backend từ chối;

- không trả private Project data;

- không phụ thuộc việc frontend có ẩn Project hay không.

Authorization phải được kiểm tra tại server theo API/Architecture contract.

# **13. QA3-PROJ-004 — Archive Project**

**Priority:** P2  
**Type:** Functional

### **Preconditions**

Project active.

### **Steps**

1.  User chọn Archive.

2.  Confirm.

### **Expected Result**

- Project chuyển trạng thái archive;

- dữ liệu không bị xóa;

- Project có thể được truy xuất theo workflow được hỗ trợ.

# **14. Test Group — Story**

## **QA3-STORY-001 — Create Story**

**Priority:** P0

### **Steps**

1.  Mở Project.

2.  Tạo Story.

3.  Nhập title/basic information.

4.  Save.

### **Expected Result**

- Story được liên kết đúng Project;

- persistence thành công;

- reopen Project vẫn load Story.

# **15. QA3-STORY-002 — Update Story**

**Priority:** P1

### **Preconditions**

Story tồn tại.

### **Steps**

1.  Edit Story data.

2.  Save.

3.  Reload.

### **Expected Result**

New canonical Story state được lưu.

Historical generation data không tự mất.

# **16. QA3-STORY-003 — Invalid Story Update**

**Priority:** P1  
**Type:** API

### **Steps**

Gửi request thiếu required field hoặc vi phạm contract.

### **Expected Result**

- HTTP thuộc nhóm 4xx;

- JSend status = fail;

- dữ liệu cũ không thay đổi.

API hiện sử dụng quy ước:

> 2xx → success, 4xx → fail, 5xx → error.

# **17. Test Group — Character**

## **QA3-CHAR-001 — Create Character**

**Priority:** P0

### **Steps**

1.  Mở Character workspace.

2.  Create Character.

3.  Nhập identity fields.

4.  Save.

### **Expected Result**

- Character được tạo;

- Character thuộc đúng Project;

- Character có stable ID;

- reopen vẫn tồn tại.

# **18. QA3-CHAR-002 — Update Character Identity**

**Priority:** P0

### **Preconditions**

Character Aria tồn tại.

### **Steps**

1.  Edit canonical identity field.

2.  Save.

### **Expected Result**

- canonical Character mới được lưu;

- revision/version thay đổi theo implementation;

- historical generation không bị xóa.

# **19. QA3-CHAR-003 — Update Appearance Only**

**Priority:** P1

### **Steps**

Thay scene-dependent appearance:

outfit

injury

equipment

### **Expected Result**

Stable identity data không bị overwrite.

# **20. QA3-CHAR-004 — Locked Attribute**

**Priority:** P1

### **Preconditions**

Một identity attribute được lock.

### **Steps**

AI/user workflow cố thay đổi qua operation không được phép.

### **Expected Result**

Locked attribute không bị thay đổi ngoài workflow cho phép.

# **21. Test Group — Character Reference**

## **QA3-REF-001 — Add Character Reference**

**Priority:** P0

### **Steps**

1.  Mở Character.

2.  Upload valid image.

3.  Save.

### **Expected Result**

- Asset được lưu;

- Reference liên kết Character;

- image load được.

# **22. QA3-REF-002 — Set Canonical Reference**

**Priority:** P0

### **Steps**

1.  Character có nhiều reference.

2.  Chọn một reference làm canonical.

### **Expected Result**

- canonical reference được xác định rõ;

- generation sau có thể truy xuất đúng reference.

# **23. QA3-REF-003 — Regeneration Must Not Replace Canonical Reference**

**Priority:** P0

### **Preconditions**

Aria-Ref-01 là canonical.

### **Steps**

1.  Generate image.

2.  Regenerate nhiều lần.

### **Expected Result**

Canonical Reference không bị tự động thay thế chỉ vì có generated Candidate mới.

# **24. QA3-REF-004 — Invalid Reference Upload**

**Priority:** P1

### **Steps**

Upload unsupported/corrupted asset.

### **Expected Result**

- request bị reject hoặc asset đánh dấu invalid theo contract;

- Character data vẫn an toàn;

- không tạo broken canonical reference.

# **25. Test Group — Scene**

## **QA3-SCENE-001 — Create Scene**

**Priority:** P0

### **Steps**

1.  Mở Story.

2.  Create Scene.

3.  Nhập Scene description.

4.  Save.

### **Expected Result**

- Scene liên kết Story;

- Scene có order/index;

- persistence thành công.

# **26. QA3-SCENE-002 — Edit Scene**

**Priority:** P0

### **Steps**

Edit Scene description.

### **Expected Result**

- new Scene state được lưu;

- old generated Candidates không bị xóa tự động.

# **27. QA3-SCENE-003 — Reorder Scene**

**Priority:** P1

### **Steps**

Thay Scene order.

### **Expected Result**

- order được persist;

- reload vẫn đúng.

Nếu order ảnh hưởng temporal context, dependent output handling phải tuân theo Domain/Data rules.

# **28. QA3-SCENE-004 — Delete Scene**

**Priority:** P1

### **Preconditions**

Scene tồn tại.

### **Expected Result**

Delete behavior phải theo business rule đã được implementation chọn.

Không được để orphan Candidate/Asset relation gây database inconsistency.

# **29. Test Group — Scene Character Assignment**

## **QA3-SC-001 — Assign Character to Scene**

**Priority:** P0

### **Steps**

1.  Mở Scene-02.

2.  Add Aria.

3.  Add Kael.

### **Expected Result**

Active Character list gồm chính xác:

Aria

Kael

# **30. QA3-SC-002 — Remove Character from Scene**

**Priority:** P1

### **Steps**

Remove Kael.

### **Expected Result**

Generation context tiếp theo không còn Kael là active Character.

Historical Candidate không bị xóa.

# **31. QA3-SC-003 — Scene Character State**

**Priority:** P0

### **Steps**

Gán state riêng:

Aria → injured

Kael → normal

### **Expected Result**

Hai SceneCharacter state được lưu độc lập.

# **32. Test Group — Concurrency**

OWNIVERSE sử dụng logical concurrency token như revision_number để tránh ghi đè dữ liệu đã bị thay đổi bởi operation khác.

# **33. QA3-CONC-001 — Valid Revision Update**

**Priority:** P1

### **Preconditions**

Character revision = 4.

### **Steps**

Request update với:

expectedRevision = 4

### **Expected Result**

- update thành công;

- revision tăng theo implementation.

# **34. QA3-CONC-002 — Stale Revision Update**

**Priority:** P0

### **Preconditions**

Server revision = 5.

Client gửi:

expectedRevision = 4

### **Expected Result**

- update bị từ chối;

- server state revision 5 không bị overwrite;

- frontend nhận conflict information phù hợp.

# **35. Test Group — Image Generation**

WF-10 quy định generation phải xác định Scene, active Characters, xây và snapshot context, tạo Job, chạy image pipeline, validation và trả Candidate.

# **36. QA3-GEN-001 — Generate Valid Scene Image**

**Priority:** P0  
**Type:** E2E / Async / Integration

### **Preconditions**

- Scene tồn tại;

- Scene có required data;

- required Character context tồn tại.

### **Steps**

1.  User chọn Generate Image.

2.  System submit request.

3.  Theo dõi Job.

4.  Worker xử lý.

5.  Candidate được tạo.

### **Expected Result**

Generation Request

→ Context

→ Snapshot

→ Job

→ Worker

→ Candidate

→ Validation

→ Completed

Candidate liên kết đúng Scene.

# **37. QA3-GEN-002 — Missing Required Generation Data**

**Priority:** P0

### **Preconditions**

Scene thiếu required Character context.

### **Steps**

User chọn Generate.

### **Expected Result**

- generation không chạy;

- system chỉ rõ missing data;

- không tạo Candidate giả;

- Scene hiện tại không thay đổi.

WF-10 yêu cầu missing data phải được thông báo trước khi generation.

# **38. QA3-GEN-003 — Generation Does Not Replace Current Selected Image**

**Priority:** P0

### **Preconditions**

Scene đã có Selected Candidate A.

### **Steps**

Start generation Candidate B.

### **Expected Result**

Trong lúc:

QUEUED

PREPARING

RUNNING

VALIDATING

Selected Candidate A vẫn còn.

WF-10 yêu cầu generation job không làm mất selected image hiện tại.

# **39. QA3-GEN-004 — Generation Uses Immutable Snapshot**

**Priority:** P0

### **Steps**

1.  Start generation với Character v3.

2.  Job bắt đầu.

3.  User sửa Character thành v4.

4.  Worker hoàn tất.

### **Expected Result**

Job đang chạy vẫn dùng snapshot v3.

Không âm thầm dùng v4 giữa generation.

# **40. QA3-GEN-005 — Correct Scene Association**

**Priority:** P0

### **Steps**

Generate từ Scene-02.

### **Expected Result**

Mọi Candidate của Job liên kết Scene-02.

Không xuất hiện dưới Scene-01 hoặc Scene-03.

# **41. Test Group — Async API**

API-02 xác định HTTP chỉ khởi tạo công việc; Job state mới theo dõi công việc và SignalR chỉ thông báo nhanh, không phải source of truth.

# **42. QA3-JOB-001 — Async Request Returns 202**

**Priority:** P0  
**Type:** API / Async

### **Steps**

Gửi:

POST /api/v1/scenes/{sceneId}/generations

### **Expected Result**

HTTP:

202 Accepted

Response chứa:

status = success

jobId

operationType

status = QUEUED

target

createdAt

API-02 yêu cầu generation request trả 202 vì công việc đã được nhận nhưng chưa hoàn tất.

# **43. QA3-JOB-002 — Job Persistence**

**Priority:** P0

### **Steps**

1.  Start Job.

2.  Refresh browser.

3.  GET Job bằng Job ID.

### **Expected Result**

Frontend vẫn truy xuất được Job.

Job không phụ thuộc SignalR connection.

# **44. QA3-JOB-003 — Standard Job Lifecycle**

**Priority:** P0

Expected lifecycle hợp lệ:

CREATED

→ QUEUED

→ CLAIMED

→ PREPARING

→ RUNNING

→ VALIDATING

→ COMPLETED

Lifecycle này được API-02 xác định cho async processing.

# **45. QA3-JOB-004 — No Invalid Transition**

**Priority:** P1

Ví dụ cố chuyển:

COMPLETED → RUNNING

### **Expected Result**

Transition bị từ chối hoặc không xảy ra.

# **46. QA3-JOB-005 — SignalR Disconnect**

**Priority:** P1

### **Steps**

1.  Start generation.

2.  Disconnect realtime channel.

3.  Job tiếp tục.

4.  Poll REST API.

### **Expected Result**

- Job không fail vì SignalR mất;

- REST vẫn trả trạng thái chính xác;

- reconnect có thể đồng bộ lại state.

# **47. Test Group — Candidate Review**

WF-11 yêu cầu Candidate hiển thị preview, status, warning, generation information và cho user Select/Regenerate/Keep/Archive.

# **48. QA3-CAND-001 — Display Candidate**

**Priority:** P0

### **Expected Result**

Candidate Card hiển thị tối thiểu:

- preview;

- status;

- selected state;

- warning nếu có.

# **49. QA3-CAND-002 — Warning Candidate Review**

**Priority:** P1

### **Preconditions**

Candidate có non-blocking warning.

### **Expected Result**

- warning visible;

- user vẫn xem được;

- user vẫn có thể select nếu business rule cho phép.

# **50. QA3-CAND-003 — Blocking Candidate**

**Priority:** P0

### **Preconditions**

Candidate validation = BLOCKING / REJECT.

### **Expected Result**

Candidate không được dùng làm selected output.

# **51. Test Group — Candidate Selection**

WF-12 quy định Scene chỉ có một selected image chính ở một thời điểm, nhưng Candidate trước đó vẫn phải còn trong history.

# **52. QA3-SEL-001 — Select Candidate**

**Priority:** P0

### **Preconditions**

Candidate A hợp lệ.

### **Steps**

User chọn:

Use this result

### **Expected Result**

- Candidate A trở thành Selected;

- Scene SelectedCandidateId cập nhật đúng.

# **53. QA3-SEL-002 — Replace Selected Candidate**

**Priority:** P0

### **Preconditions**

Candidate A đang selected.

### **Steps**

Select Candidate B.

### **Expected Result**

- B trở thành selected;

- A mất selected flag;

- A vẫn tồn tại trong history.

# **54. QA3-SEL-003 — Re-select Previous Candidate**

**Priority:** P1

### **Preconditions**

A từng selected, sau đó B selected.

### **Steps**

User chọn lại A.

### **Expected Result**

A trở lại selected nếu vẫn hợp lệ.

# **55. QA3-SEL-004 — Select Blocking Candidate**

**Priority:** P0

### **Expected Result**

Operation bị từ chối.

Scene selected output hiện tại không thay đổi.

# **56. Test Group — Regeneration**

WF-13 quy định regeneration tạo output mới mà không overwrite output cũ; hỗ trợ same input, modified instruction hoặc updated canonical context.

# **57. QA3-REG-001 — Regenerate Same Input**

**Priority:** P0

### **Steps**

1.  Candidate A tồn tại.

2.  User chọn Regenerate.

3.  Không đổi instruction/context.

### **Expected Result**

- new generation operation được tạo;

- Candidate mới không overwrite A;

- history chứa cả hai.

# **58. QA3-REG-002 — Regenerate with Modified Instruction**

**Priority:** P1

### **Steps**

Instruction:

Use a wider shot.

### **Expected Result**

- instruction mới được lưu cùng generation;

- old Candidate giữ nguyên;

- Candidate mới trace được instruction mới.

# **59. QA3-REG-003 — Regenerate with Updated Canonical Context**

**Priority:** P0

### **Preconditions**

Character v3 → v4.

### **Steps**

Regenerate bằng current canonical state.

### **Expected Result**

New generation snapshot dùng v4.

Old Candidate tiếp tục trace tới v3.

# **60. QA3-REG-004 — Regeneration Is Not Retry**

**Priority:** P0

User chủ động regenerate không được ghi như technical retry của một transient failure.

Nguồn hiện tại khẳng định regeneration và retry là hai operation khác nhau.

**Lưu ý thuật ngữ:** WF-13 gọi kết quả user-requested regeneration là một Generation Attempt mới, trong khi API-02 mô tả regeneration là một creative operation mới. QA phải bảo đảm hai hành vi **không bị đồng nhất với retry**; mapping Job/Attempt cụ thể phải theo contract API/runtime cuối cùng được triển khai.

# **61. Test Group — Retry**

## **QA3-RETRY-001 — Retry Transient Failure**

**Priority:** P0

### **Scenario**

Worker/provider gặp temporary technical failure.

### **Expected Result**

- retry tuân theo retry policy;

- retry không bị hiển thị như user regeneration;

- retry count/attempt metadata được lưu.

# **62. QA3-RETRY-002 — Retry Limit**

**Priority:** P1

### **Steps**

Giả lập failure liên tục.

### **Expected Result**

Sau configured retry limit:

Job → FAILED

Không infinite retry.

# **63. Test Group — Canonical Change / Staleness**

WF-14 yêu cầu canonical change tạo version mới, giữ existing generations và có thể đánh dấu chúng OUTDATED_CONTEXT; user là người quyết định có regenerate hay không.

# **64. QA3-STALE-001 — Character Change Marks Affected Candidate**

**Priority:** P0

### **Preconditions**

Candidate A được sinh từ Character v3.

### **Steps**

Character cập nhật → v4.

### **Expected Result**

Nếu Candidate phụ thuộc field thay đổi:

- Candidate A vẫn tồn tại;

- hệ thống nhận biết context cũ;

- Candidate có trạng thái outdated/stale tương ứng.

# **65. QA3-STALE-002 — Canonical Update Must Not Delete History**

**Priority:** P0

### **Expected Result**

Character/Story/Scene update không xóa generated Candidate cũ.

# **66. QA3-STALE-003 — No Automatic Whole-Story Regeneration**

**Priority:** P1

### **Steps**

Thay Character canonical data.

### **Expected Result**

System không tự động regenerate toàn story trừ khi có explicit workflow riêng.

User được quyền quyết định.

# **67. QA3-STALE-004 — Stale Candidate Still Traceable**

**Priority:** P1

### **Expected Result**

Có thể biết Candidate được sinh từ:

Character version

Scene version

Story/Core version

Generation Job

# **68. Test Group — Generation History**

WF-15 yêu cầu attempts được hiển thị theo thời gian và history không được mất sau regeneration.

# **69. QA3-HIST-001 — History Survives Regeneration**

**Priority:** P0

### **Steps**

Generate A → Regenerate B → Regenerate C.

### **Expected Result**

History chứa:

A

B

C

đúng thứ tự.

# **70. QA3-HIST-002 — History Shows Metadata**

**Priority:** P1

Mỗi history item nên hiển thị theo contract hỗ trợ:

- Candidate;

- status;

- timestamp;

- instruction;

- selected state;

- warning.

# **71. QA3-HIST-003 — Current Candidate Traceability**

**Priority:** P0

User phải xác định được selected Candidate hiện tại đến từ generation nào.

# **72. QA3-HIST-004 — History Survives Application Restart**

**Priority:** P0

### **Steps**

1.  Generate nhiều Candidates.

2.  Restart application/backend.

3.  Reopen Scene.

### **Expected Result**

Generation history vẫn tồn tại.

# **73. Test Group — JSend API Contract**

OWNIVERSE chuẩn hóa HTTP API theo JSend.

# **74. QA3-API-001 — Successful Response**

**Priority:** P1

Expected:

2xx

status = "success"

data = ...

# **75. QA3-API-002 — Validation Failure**

**Priority:** P1

Expected:

4xx

status = "fail"

errorKey / validation information

Frontend không parse free-text message để quyết định business logic.

# **76. QA3-API-003 — Infrastructure Error**

**Priority:** P1

Expected:

5xx

status = "error"

Không trả business success khi operation thực tế thất bại.

# **77. QA3-API-004 — Realtime Event Is Not JSend**

**Priority:** P2

SignalR/realtime event không cần bọc JSend vì không phải HTTP response.

# **78. Test Group — Persistence**

## **QA3-DATA-001 — Candidate Persistence**

**Priority:** P0

Candidate đã lưu phải tồn tại sau:

- refresh;

- logout/login;

- backend restart.

# **79. QA3-DATA-002 — Selected Candidate Persistence**

**Priority:** P0

Selected image của Scene phải giữ sau reload.

# **80. QA3-DATA-003 — Snapshot Persistence**

**Priority:** P0

Generation hoàn tất phải truy được context snapshot tương ứng.

# **81. QA3-DATA-004 — Historical Version Immutability**

**Priority:** P1

Một Character/Scene version đã được generation sử dụng không được bị edit ngược khiến historical provenance thay đổi.

Nguồn Data hiện xác định historical version đã dùng cho generation nên được xem là immutable record.

# **82. Test Group — Storage**

## **QA3-ASSET-001 — Candidate Asset Exists**

**Priority:** P0

Candidate có image reference thì Asset thực tế phải tồn tại.

# **83. QA3-ASSET-002 — Asset Ownership**

**Priority:** P0

User-B không được lấy private asset thuộc Project User-A bằng cách đoán Asset ID/URL.

# **84. QA3-ASSET-003 — Storage Failure During Generation**

**Priority:** P0

### **Scenario**

AI tạo image thành công nhưng storage save thất bại.

### **Expected Result**

Job không được báo COMPLETED như thể Candidate đã lưu an toàn.

Không tạo Candidate trỏ tới nonexistent asset.

# **85. Test Group — Worker / Recovery**

## **QA3-REC-001 — Worker Crash During Running Job**

**Priority:** P0

### **Expected Result**

- Job không biến thành Completed;

- runtime có thể phát hiện failure/timeout;

- retry/recovery tuân theo ARCH/API policy.

# **86. QA3-REC-002 — Backend Restart While Job Running**

**Priority:** P1

### **Expected Result**

Persistent Job state còn trong database.

Worker operation không phụ thuộc browser/backend memory thuần túy.

# **87. QA3-REC-003 — Browser Refresh During Generation**

**Priority:** P0

### **Expected Result**

Sau refresh:

GET Job

→ current state

Frontend tiếp tục hiển thị đúng progress/result.

# **88. QA3-REC-004 — Queue Temporarily Unavailable**

**Priority:** P1

### **Expected Result**

System trả failure có thể quan sát.

Không silently mất generation request.

# **89. Critical End-to-End Test**

## **QA3-E2E-001 — Main MVP Journey**

**Priority:** P0

### **Steps**

1.  Create Project.

2.  Create Story.

3.  Create Character Aria.

4.  Upload canonical Character Reference.

5.  Create Scene.

6.  Assign Aria.

7.  Define Scene state.

8.  Generate Image.

9.  Receive Job ID.

10. Observe Job processing.

11. Candidate returned.

12. Review Candidate.

13. Select Candidate.

14. Reload application.

15. Reopen Scene.

### **Expected Result**

- toàn bộ workflow hoàn thành;

- selected Candidate vẫn tồn tại;

- Character/Scene data không mất;

- history tồn tại;

- Candidate trace được Job.

# **90. QA3-E2E-002 — Multi-Character Journey**

**Priority:** P0

### **Steps**

1.  Create Aria.

2.  Create Kael.

3.  Add canonical references.

4.  Create Scene.

5.  Assign cả hai.

6.  Generate.

7.  Review validation.

8.  Select Candidate.

### **Expected Result**

Functional workflow xử lý được multi-character context.

Visual identity quality thuộc QA-02.

# **91. QA3-E2E-003 — Regeneration Journey**

**Priority:** P0

### **Steps**

Generate A

→ Select A

→ Regenerate B

→ Review B

→ Select B

→ Open History

→ Select A again

### **Expected Result**

- không mất A;

- B không overwrite A;

- selected state luôn duy nhất;

- history chính xác.

# **92. QA3-E2E-004 — Canonical Change Journey**

**Priority:** P0

### **Steps**

Generate using Aria v3

↓

Select Candidate A

↓

Edit Aria → v4

↓

Candidate A becomes stale/outdated where applicable

↓

Regenerate

↓

Candidate B uses v4

### **Expected Result**

A và B đều tồn tại và có provenance riêng.

# **93. Smoke Regression Suite**

Sau mỗi major build/deployment nên chạy tối thiểu:

QA3-PROJ-001

QA3-CHAR-001

QA3-REF-001

QA3-SCENE-001

QA3-SC-001

QA3-GEN-001

QA3-JOB-001

QA3-CAND-001

QA3-SEL-001

QA3-HIST-001

QA3-E2E-001

# **94. Automation Priority**

## **Strong Automation Candidates**

Nên tự động hóa sớm:

Project CRUD

Character CRUD

Scene CRUD

API JSend

Revision conflict

Job lifecycle

Candidate persistence

Selection

History

Stale detection

Authorization

# **95. Partial Automation Candidates**

Có thể dùng fake/mocked worker:

Generation request

Job lifecycle

Candidate creation

Retry

Regeneration

Không cần chạy diffusion model thật.

# **96. Real AI Integration Suite**

Một suite nhỏ dùng real worker/model cho:

AI worker connection

Image generation

Asset save

Candidate creation

Validation

Completion

Không cần chạy trên mọi commit.

# **97. Requirement Traceability Matrix**

Ví dụ:

| **Test**      | **Source** | **Main Rule**        |
|---------------|------------|----------------------|
| QA3-GEN-001   | WF-10      | Generate Scene Image |
| QA3-CAND-001  | WF-11      | Review Candidates    |
| QA3-SEL-001   | WF-12      | Select Candidate     |
| QA3-REG-001   | WF-13      | Regeneration         |
| QA3-STALE-001 | WF-14      | Canonical Changes    |
| QA3-HIST-001  | WF-15      | Generation History   |
| QA3-JOB-001   | API-02     | Async Job            |
| QA3-CONC-002  | DATA/API   | Revision Conflict    |

# **98. Test Result Record**

Mỗi execution lưu:

testId

buildVersion

environment

executedAt

result

PASS

FAIL

BLOCKED

SKIPPED

evidence

defectId

notes

# **99. Exit Criteria**

QA-03 được xem là đạt cho MVP Release Candidate khi:

### **Functional**

- tất cả P0 functional tests PASS;

- core P1 tests PASS hoặc có accepted limitation.

### **Integration**

- Backend ↔ Database PASS;

- Backend ↔ Queue PASS;

- Queue ↔ Worker PASS;

- Worker ↔ Storage PASS.

### **Async**

- Job create/persist/update PASS;

- browser refresh/reconnect PASS.

### **Data Integrity**

- Candidate history không mất;

- selected Candidate persist;

- stale/version behavior hoạt động.

### **Security**

- cross-project unauthorized access bị chặn.

# **100. Release Blocking Conditions**

Không được release/demo nếu còn các failure như:

Project data loss

Character/Scene data corruption

Selected Candidate disappears

Blocking Candidate can be selected

Generation Job disappears after refresh

Candidate saved under wrong Scene

Unauthorized Project access

Regeneration overwrites history

Storage failure reported as successful completion

# **101. Relationship with QA-02**

QA-03 hỏi:

> **Pipeline có làm đúng workflow không?**

QA-02 hỏi:

> **AI output và validator có đủ consistency không?**

Ví dụ:

QA-03 PASS:

Candidate generated and stored correctly.

QA-02 FAIL:

Character identity is wrong.

Đây là kết quả hoàn toàn hợp lệ.

# **102. Relationship with QA-04**

QA-03 xác nhận:

> hệ thống hoạt động theo specification.

QA-04 sẽ xác nhận:

> người dùng thật có thể sử dụng hệ thống để hoàn thành mục tiêu MVP và build đã đủ điều kiện demo/release chưa.

# **103. Final Principle**

Nguyên tắc của QA-03:

> **Mọi critical user action phải tạo ra một system state có thể kiểm chứng, persist và truy dấu.**

Luồng chính cần được bảo vệ xuyên suốt:

> **Project → Story → Character → Reference → Scene → Context → Job → Worker → Candidate → Validation → Selection → History**

Và khi có lỗi:

> **Failure không được làm mất canonical data, selected output hoặc generation history.**

QA-03 vì vậy là tài liệu trực tiếp nhất để biến specification của OWNIVERSE thành **automated tests, integration tests và regression suite trong quá trình code**.
