# **QA-01 — Master Test Plan & Quality Strategy**

**Document ID:** QA-01  
**Document Type:** Master Test Plan & Quality Strategy  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Status:** Draft v1  
**Primary Roles:** QA Engineer / Developer / AI Engineer / Thesis Researcher  
**Parent Documents:** PROD-01, PROD-02, ARCH-01, ARCH-02  
**Related Documents:** QA-02, QA-03, QA-04, AI-03, API-01, API-02, RESEARCH-04

# **1. Purpose**

QA-01 xác định chiến lược kiểm thử tổng thể của OWNIVERSE.

Tài liệu trả lời các câu hỏi:

- hệ thống cần được kiểm thử ở những mức nào;

- module nào cần test;

- test nào nên tự động hóa;

- test nào cần đánh giá thủ công;

- AI output được kiểm tra như thế nào;

- lỗi được phân loại ra sao;

- khi nào một feature được xem là hoàn thành;

- khi nào hệ thống đủ điều kiện để demo hoặc release;

- QA của sản phẩm khác gì với experimental evaluation của luận văn.

Tài liệu gốc đã xác định Testing không nên để tới gần thời điểm bảo vệ mới thực hiện và phải bao phủ Test Plan, Functional Test Cases, AI Evaluation, Image Evaluation và User Acceptance Test.  
QA-01 đóng vai trò master document điều phối toàn bộ các hoạt động đó.

# **2. Quality Objective**

Mục tiêu của QA không chỉ là:

> “Ứng dụng không crash.”

OWNIVERSE phải đảm bảo đồng thời:

### **Functional Correctness**

Feature hoạt động đúng requirement.

### **Data Correctness**

Canonical state, version, snapshot, Candidate và Asset không bị sai hoặc mất.

### **Integration Correctness**

Frontend, Backend, Database, Queue và AI Worker giao tiếp đúng contract.

### **AI Workflow Correctness**

Generation context đúng character, đúng scene và đúng version.

### **Reliability**

Job có thể retry, cancel, recover và không mất trạng thái.

### **User Experience Correctness**

User có thể hoàn thành workflow chính mà không gặp blocker.

### **Research Support Correctness**

Generation metadata đủ để tái hiện experiment.

# **3. Testing Scope**

Testing bao phủ các thành phần chính:

Frontend

Backend

Database

Asset Storage

Job Queue

AI Worker

Image Generation Adapter

Realtime Notification

Consistency Validation

Research Metadata

Luồng tổng quát cần được test end-to-end:

> **User → Application → Job → AI → Validation → Candidate → User Decision → Canonical State**

Luồng này cũng chính là backbone kỹ thuật được Architecture Specification xác định.

# **4. QA Document Structure**

Bộ QA gồm bốn tài liệu.

## **QA-01 — Master Test Plan & Quality Strategy**

Định nghĩa:

> kiểm thử cái gì, bằng cách nào và điều kiện chất lượng chung.

## **QA-02 — AI, Image & Consistency Evaluation Specification**

Định nghĩa:

- character consistency;

- scene correctness;

- identity leakage;

- long-range recurrence;

- image quality;

- AI validation.

## **QA-03 — Functional & Integration Test Case Specification**

Định nghĩa test case chi tiết cho:

- Project;

- Story;

- Character;

- Scene;

- Candidate;

- API;

- Job;

- Versioning;

- storage;

- integration.

## **QA-04 — User Acceptance & Release Readiness Specification**

Định nghĩa:

- UAT;

- critical user journeys;

- demo readiness;

- release checklist.

# **5. Relationship with Research Evaluation**

Phải phân biệt:

### **Product QA**

Hỏi:

> OWNIVERSE có hoạt động đúng không?

### **Research Evaluation**

Hỏi:

> SAMIM có tốt hơn baseline một cách khoa học không?

RESEARCH-04 phụ trách:

- baseline comparison;

- controlled experiment;

- metrics;

- ablation;

- hypothesis testing.

QA-02 phụ trách:

> AI output của ứng dụng có đáp ứng requirement vận hành hay không.

Không được sử dụng Product QA như bằng chứng thay cho scientific experiment.

# **6. Test Levels**

OWNIVERSE sử dụng sáu mức kiểm thử chính.

Unit Test

↓

Module Test

↓

Integration Test

↓

System Test

↓

AI / Visual Evaluation

↓

User Acceptance Test

Mỗi mức giải quyết một loại risk khác nhau.

# **7. Unit Testing**

Unit Test kiểm tra business logic nhỏ độc lập.

Các đối tượng phù hợp:

- validators;

- domain services;

- state transition logic;

- version comparison;

- stale detection;

- prompt/context helper;

- Job state transition;

- permission logic;

- mapper;

- utility functions.

Ví dụ:

CanSelectCandidate()

CanCancelJob()

IsCandidateStale()

ResolveCharacterVersion()

ValidateSceneCharacters()

# **8. Unit Test Principle**

Unit test phải:

- deterministic;

- nhanh;

- không phụ thuộc AI model thật;

- không cần network;

- không phụ thuộc external API.

AI provider nên được mock hoặc fake ở unit-test level.

# **9. Backend Unit Testing**

Các module backend ưu tiên unit test:

Project

Story

Character

Scene

Generation

Candidate

Job

Versioning

Authorization

Đặc biệt ưu tiên logic có nguy cơ làm hỏng canonical state.

# **10. Frontend Unit Testing**

Frontend unit test nên tập trung vào:

- state logic;

- form validation;

- data transformation;

- component behavior quan trọng;

- Job status rendering;

- API error handling.

Không cần unit-test mọi visual detail.

# **11. Integration Testing**

Integration Test kiểm tra nhiều component thật làm việc cùng nhau.

Ví dụ:

API

\+

Database

hoặc:

Backend

\+

Queue

\+

Worker

hoặc:

Backend

\+

Object Storage

# **12. Database Integration Tests**

Cần kiểm tra:

- persistence;

- transaction;

- constraint;

- foreign key;

- version relationship;

- deletion rules;

- snapshot integrity.

Ví dụ:

> Xóa Character đang được Scene sử dụng có được phép hay không?

Kết quả phải phù hợp Domain Specification.

# **13. API Integration Tests**

API-01 được kiểm tra về:

- endpoint;

- authorization;

- request validation;

- response structure;

- JSend status;

- revision conflict;

- resource ownership.

Ví dụ:

POST /api/v1/projects

thành công phải trả:

status = success

Invalid business request phải trả:

status = fail

Infrastructure failure tương ứng:

status = error

# **14. Async Integration Tests**

API-02 và ARCH-02 yêu cầu kiểm thử riêng cho asynchronous workflow.

Các test phải bao gồm:

Request generation

→ Job created

→ Queue

→ Worker receives

→ Attempt runs

→ Candidate stored

→ Job completed

→ Frontend observes result

# **15. Job State Testing**

Cần kiểm tra các state transition hợp lệ.

Ví dụ:

QUEUED

→ PREPARING

→ RUNNING

→ VALIDATING

→ COMPLETED

và các nhánh:

RUNNING

→ FAILED

QUEUED

→ CANCELLED

Transition không hợp lệ phải bị từ chối.

# **16. Retry Testing**

Retry phải được phân biệt với regeneration.

### **Retry**

Cùng Job nhưng tạo Attempt mới do technical failure.

### **Regeneration**

User chủ động yêu cầu creative generation mới → Job mới.

QA phải bảo đảm hai hành vi này không bị nhầm.

# **17. Worker Failure Testing**

ARCH-02 yêu cầu runtime hỗ trợ fault recovery, health check, graceful shutdown và monitoring.

Do đó cần test:

- worker crash;

- worker timeout;

- lost heartbeat;

- queue reconnect;

- backend restart;

- worker restart.

Job không được mất trạng thái vô lý.

# **18. Context Snapshot Testing**

Một Job đang chạy phải sử dụng immutable context snapshot.

Test case quan trọng:

Generation starts using Character v3

↓

User changes Character to v4

↓

Running Job continues using v3 snapshot

Job không được âm thầm chuyển sang v4 giữa quá trình generation.

# **19. Candidate Staleness Testing**

Sau khi Character hoặc Scene thay đổi:

Candidate cũ có thể trở thành:

CURRENT

POTENTIALLY_STALE

STALE

tùy rule được Data Specification xác định.

QA phải kiểm tra stale detection không làm mất Candidate cũ.

# **20. Asset Testing**

Asset tests bao gồm:

- upload;

- download;

- ownership;

- reference linking;

- deletion;

- inaccessible asset;

- broken path;

- object storage unavailable.

Canonical private asset không được mặc định public, phù hợp architecture hiện tại.

# **21. End-to-End Testing**

E2E Test kiểm tra user workflow hoàn chỉnh.

Core journey:

Create Project

↓

Create Story

↓

Create Character

↓

Add Character Reference

↓

Create Scene

↓

Assign Character

↓

Generate Image

↓

Wait for Job

↓

Review Candidate

↓

Select Candidate

Nếu journey này không hoàn thành:

> MVP chưa đạt release readiness.

# **22. Core MVP Test Areas**

MVP QA phải bao phủ tối thiểu:

1.  authentication/authorization nếu có;

2.  project management;

3.  story management;

4.  character management;

5.  character reference;

6.  scene management;

7.  scene-character assignment;

8.  AI-assisted generation;

9.  image generation;

10. Job tracking;

11. Candidate management;

12. regeneration;

13. generation history;

14. consistency validation;

15. persistence.

# **23. Character Management Tests**

Kiểm tra:

- create;

- edit;

- stable identity;

- visual state;

- canonical reference;

- locked attribute;

- history/version.

Thay đổi outfit không được vô tình thay đổi identity data.

# **24. Scene Tests**

Kiểm tra:

- create;

- edit;

- delete;

- reorder;

- assign Character;

- remove Character;

- scene description;

- visual state resolution;

- context generation.

# **25. Multi-Character Tests**

Scene chứa:

Character A

\+

Character B

phải tạo đúng generation context cho cả hai.

Validation phải có khả năng kiểm tra:

- A xuất hiện;

- B xuất hiện;

- identity không bị trộn;

- scene intent được giữ.

Đây cũng là workflow đã được system specification xác định.

# **26. AI Testing Strategy**

AI output là non-deterministic.

Do đó không nên test:

GeneratedImage == expected.png

Thay vào đó test ba lớp.

### **Layer 1 — Deterministic Pipeline**

Context đúng hay không.

### **Layer 2 — Structural Validation**

Output có tồn tại, đúng format, metadata đầy đủ.

### **Layer 3 — Quality Evaluation**

Character/scene/image có đạt chất lượng yêu cầu không.

Layer 3 được QA-02 đặc tả.

# **27. AI Provider Mock**

Trong functional test:

AI Provider có thể được mock.

Ví dụ:

FakeImageGenerator

luôn trả một known Asset.

Mục tiêu:

> kiểm tra workflow application chứ không phải chất lượng model.

# **28. Real AI Test**

Một số system tests phải chạy model thật để kiểm tra:

- Worker;

- model loading;

- GPU;

- storage;

- timeout;

- Candidate creation;

- validation;

- metadata.

Nhưng không cần chạy model thật cho mọi CI test.

# **29. Research Metadata Testing**

Generation pipeline phục vụ luận văn phải lưu metadata như:

- recurring Character IDs;

- Character count;

- Scene position;

- distance từ previous appearance;

- generation attempt;

- consistency validation result.

QA phải kiểm tra metadata này được ghi đúng, vì thiếu metadata sẽ làm experiment không reproducible.

# **30. Reproducibility Testing**

Một experiment record tối thiểu phải truy ra được:

Model

Checkpoint

Method Version

Context Snapshot

Generation Parameters

Seed

Output

Nếu applicable.

Không có provenance:

> output không đủ tin cậy cho research analysis.

# **31. Test Types**

Bộ test tổng thể gồm:

Functional

Integration

API

Database

Async Job

AI

Visual

Security

Performance

Recovery

UAT

Không nhất thiết mọi loại đều có quy mô production-grade trong luận văn MVP.

# **32. Functional Testing**

Kiểm tra:

> Requirement có được thực thi đúng hay không?

Nguồn chính:

- PROD-01;

- PROD-02;

- DOMAIN-01;

- DOMAIN-02.

Chi tiết từng test case nằm trong QA-03.

# **33. Contract Testing**

Kiểm tra Backend và Frontend tuân thủ API contract.

Ví dụ:

- JSON field;

- enum;

- JSend format;

- errorKey;

- pagination;

- revision.

Mục tiêu:

> tránh frontend/backend hiểu contract khác nhau.

# **34. Security Testing**

MVP cần kiểm tra tối thiểu:

- authorization;

- object ownership;

- unauthorized project access;

- private asset access;

- input validation;

- secret exposure.

Không yêu cầu penetration testing enterprise-level cho luận văn, trừ khi scope thay đổi.

# **35. Performance Testing**

Performance tests tập trung vào workflow có ảnh hưởng rõ tới user.

Ví dụ:

- project loading;

- story loading;

- API response;

- Job creation;

- upload reference;

- Job polling.

AI inference latency nên được đo riêng khỏi ordinary API latency.

# **36. Load Testing**

MVP không yêu cầu massive-scale load testing.

Nhưng nên kiểm tra:

- nhiều Job queued;

- nhiều Candidate;

- multiple simultaneous generations;

- queue không làm mất Job.

ARCH-02 đã xác định resource management và GPU scheduling là runtime concern.

# **37. Recovery Testing**

Các scenario cần kiểm tra:

Backend restart

Worker restart

Queue restart

Storage temporary failure

Database temporary failure

Mục tiêu:

> hệ thống không biến Job thành trạng thái không thể giải thích.

# **38. Test Environment**

Tối thiểu ba environment logic:

Development

Testing

Demo / Production-like

ARCH-02 cũng đã yêu cầu phân biệt local development, thesis demo và production-like environment.

# **39. Development Environment**

Dùng cho:

- coding;

- unit test;

- manual testing;

- local AI integration.

Có thể sử dụng Docker Compose cho infrastructure local, phù hợp định hướng Deployment hiện tại.

# **40. Testing Environment**

Nên có:

- clean database;

- deterministic seed data;

- isolated storage;

- fake/mock AI khi cần;

- real AI configuration cho integration test chọn lọc.

# **41. Demo Environment**

Demo thesis phải gần production behavior hơn development.

Không nên phụ thuộc:

- IDE đang mở;

- manually launched random script;

- hidden local configuration;

- undocumented setup.

Deployment documents sẽ chuẩn hóa phần này.

# **42. Test Data**

Test data gồm:

Users

Projects

Stories

Characters

References

Scenes

Candidates

Jobs

Assets

Cần có reusable seed data.

# **43. Standard Test Story**

Nên tạo một standard story fixture có:

- ít nhất hai recurring Characters;

- single-character Scene;

- multi-character Scene;

- Character vắng mặt rồi quay lại;

- outfit/state change.

Fixture này dùng xuyên QA-02 và QA-03.

# **44. Test Isolation**

Mỗi automated test không nên phụ thuộc order của test trước.

Test phải tự:

- setup;

- execute;

- verify;

- cleanup/reset.

# **45. Test Case Structure**

Mỗi test case QA-03 sử dụng format:

Test ID

Requirement ID

Title

Preconditions

Test Data

Steps

Expected Result

Priority

Test Type

Automation Status

Result

Evidence

# **46. Requirement Traceability**

Mỗi critical requirement nên trace được:

Requirement

↓

Implementation

↓

Test Case

↓

Test Result

Ví dụ:

PR-FR-11 Character Reference

↓

Character Module

↓

QA3-CHAR-REF-01

↓

PASS

# **47. Traceability Rule**

Không cần tạo test case riêng cho từng câu văn trong specification.

Ưu tiên:

- critical requirement;

- business rule;

- data integrity;

- risky integration;

- main workflow.

# **48. Priority Levels**

Test Priority:

P0 — Critical

P1 — High

P2 — Medium

P3 — Low

### **P0**

Feature không thể demo nếu fail.

### **P1**

Core feature bị ảnh hưởng mạnh.

### **P2**

Feature phụ hoặc edge case.

### **P3**

Cosmetic / low-impact behavior.

# **49. Defect Severity**

Defect dùng:

S0 — Blocker

S1 — Critical

S2 — Major

S3 — Minor

S4 — Cosmetic

# **50. S0 — Blocker**

Ví dụ:

- app không start;

- database migration fail;

- login/core access fail;

- generation workflow hoàn toàn không hoạt động.

Không thể tiếp tục test.

# **51. S1 — Critical**

Ví dụ:

- mất canonical data;

- user đọc Project của user khác;

- Job complete nhưng mất Candidate;

- Candidate chọn sai Character/Scene;

- corruption database.

# **52. S2 — Major**

Ví dụ:

- regenerate không hoạt động;

- history sai;

- realtime không update nhưng polling còn hoạt động;

- stale detection sai.

Core workflow vẫn có workaround.

# **53. S3 — Minor**

Ví dụ:

- validation message chưa rõ;

- refresh UI không tối ưu;

- một filter phụ sai.

# **54. S4 — Cosmetic**

Ví dụ:

- spacing;

- icon;

- text alignment;

- typo.

# **55. Test Result Status**

Mỗi test có:

NOT_RUN

PASS

FAIL

BLOCKED

SKIPPED

SKIPPED phải có reason.

Không được dùng SKIPPED để che test fail.

# **56. Automation Strategy**

Ưu tiên automation cho:

- unit tests;

- API tests;

- domain/business rules;

- database integration;

- Job state transitions;

- critical E2E flow.

Manual testing ưu tiên:

- visual UX;

- AI output quality;

- image consistency;

- exploratory testing;

- UAT.

# **57. Test Pyramid**

Định hướng:

UAT

/ \\

E2E AI

/ \\

Integration / API

\\ /

Unit Tests

Số lượng Unit/Integration tests nên lớn hơn full E2E tests vì chúng nhanh và dễ debug hơn.

# **58. CI Quality Gate**

Khi có CI pipeline, một commit/PR không nên được xem là healthy nếu:

- build fail;

- unit test fail;

- critical integration test fail;

- migration validation fail.

Full AI generation không bắt buộc chạy trên mọi commit vì cost cao.

# **59. AI Test Scheduling**

Real model test có thể chạy:

- manual;

- pre-demo;

- nightly nếu infrastructure cho phép;

- trước release candidate.

Không cần đưa GPU inference vào mọi developer feedback cycle.

# **60. Definition of Done**

Một feature được xem là DONE khi:

1.  requirement được implement;

2.  build thành công;

3.  unit test cần thiết pass;

4.  integration test cần thiết pass;

5.  QA-03 critical cases pass;

6.  không có unresolved S0/S1 defect;

7.  documentation/API contract được cập nhật nếu thay đổi.

# **61. Entry Criteria for System Testing**

System testing bắt đầu khi:

- frontend/backend build được;

- database migration ổn định;

- core API hoạt động;

- queue/worker kết nối;

- storage hoạt động;

- seed/test data có sẵn.

# **62. Exit Criteria for System Testing**

MVP system testing đạt khi:

- tất cả P0 tests pass;

- core P1 tests pass;

- không còn S0;

- không còn S1 chưa được chấp nhận;

- main E2E journey hoàn thành;

- generation workflow chạy end-to-end;

- UAT có thể bắt đầu.

# **63. Thesis Demo Readiness**

Một build được xem là đủ điều kiện demo khi:

Application starts reliably

Database initialized

AI Worker ready

Core Project workflow works

Character Reference works

Scene works

Generation works

Candidate returned

History persists

Selected Candidate survives refresh/restart

và có fallback plan nếu external AI/model infrastructure gặp vấn đề.

# **64. Demo Data**

Nên có một demo Project chuẩn đã chuẩn bị trước:

Story

Characters

References

Scenes

Generated Candidates

nhưng hệ thống cũng phải chứng minh có thể tạo workflow mới chứ không chỉ mở dữ liệu có sẵn.

# **65. Regression Testing**

Khi sửa:

- Character;

- Scene;

- Versioning;

- Job;

- AI context;

phải chạy lại những test có dependency liên quan.

Không cần manual regression toàn hệ thống sau mọi thay đổi nhỏ.

# **66. Regression Set**

Nên có một smoke regression suite gồm:

Create Project

Create Character

Add Reference

Create Scene

Assign Character

Generate

Job Complete

Candidate Saved

Select Candidate

Reload Project

Đây là regression backbone của MVP.

# **67. Smoke Test**

Sau mỗi deployment:

1.  frontend accessible;

2.  backend health check pass;

3.  database accessible;

4.  storage accessible;

5.  queue accessible;

6.  worker accessible;

7.  basic API works.

Real AI generation có thể là extended smoke test.

# **68. Observability for QA**

Để debug test failure, hệ thống nên có:

- request ID;

- Job ID;

- Attempt ID;

- timestamps;

- structured logs;

- method/model version.

Nếu Job fail nhưng không trace được log:

> QA rất khó xác định lỗi nằm ở backend, queue hay worker.

# **69. Evidence**

Critical test failure/success nên lưu evidence khi phù hợp:

- screenshot;

- request/response;

- Job ID;

- log excerpt;

- generated Asset;

- test output.

# **70. Known Limitation Handling**

Nếu một limitation chưa sửa trước demo:

phải được ghi:

Known Limitation

Impact

Workaround

Reason

Future Resolution

Không nên giả vờ behavior đó là expected nếu thực chất là bug.

# **71. QA vs Product Requirement**

QA không được tự định nghĩa business rule mới.

Expected result phải xuất phát từ:

- Product Specification;

- Domain Specification;

- API Contract;

- AI Specification.

Nếu các tài liệu mâu thuẫn:

> specification phải được giải quyết trước khi kết luận PASS/FAIL.

# **72. QA vs AI Research**

QA-02 có thể báo:

> Character consistency chưa đạt product threshold.

RESEARCH-04 có thể đồng thời kết luận:

> SAMIM cải thiện có ý nghĩa so với baseline.

Hai kết luận không mâu thuẫn.

Một model có thể tốt hơn baseline nhưng vẫn chưa đủ tốt cho production.

# **73. Responsibilities**

### **Developer**

- unit/integration tests;

- fix defect;

- reproduce technical issue.

### **QA**

- test strategy;

- test case;

- regression;

- defect verification.

### **AI Engineer / Researcher**

- AI evaluation;

- model diagnostics;

- reproducibility.

### **Product Owner / Thesis Author**

- requirement clarification;

- UAT;

- release decision.

Trong luận văn cá nhân, một người có thể đảm nhiệm nhiều role nhưng responsibility vẫn nên được phân biệt về mặt quy trình.

# **74. Quality Gate Before Merge**

Code thay đổi critical module nên đảm bảo:

Build PASS

Unit PASS

Relevant Integration PASS

No introduced critical defect

# **75. Quality Gate Before Release Candidate**

P0 Tests = PASS

Critical E2E = PASS

No S0

No unresolved S1

Database migration verified

AI workflow verified

Deployment smoke test PASS

# **76. Quality Gate Before Thesis Demo**

Release Candidate stable

Demo environment reproducible

Standard demo project available

Core generation tested

Recovery procedure known

Backup available

Known limitations documented

# **77. Outputs of QA-01**

QA-01 tạo framework cho ba tài liệu sau:

QA-01

│

├── QA-02 AI / Image / Consistency Evaluation

│

├── QA-03 Functional & Integration Test Cases

│

└── QA-04 UAT & Release Readiness

# **78. Final QA Principle**

Nguyên tắc chính của QA cho OWNIVERSE là:

> **Test deterministic software behavior deterministically, and evaluate non-deterministic AI behavior through controlled criteria rather than exact-output matching.**

Đồng thời:

> **A feature is not complete merely because it works once on the developer's machine.**

Nó phải có khả năng:

- được tái hiện;

- được kiểm tra;

- thất bại theo cách có thể quan sát;

- phục hồi theo contract;

- và hoạt động lại trong environment khác.

QA-01 vì vậy là nền tảng biến toàn bộ specification của OWNIVERSE từ:

> **“hệ thống được thiết kế như thế nào”**

thành:

> **“làm sao biết hệ thống đã được triển khai đúng.”**
