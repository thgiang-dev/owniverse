# **PROD-02 — Functional & Workflow Specification**

**Document ID:** PROD-02  
**Document Type:** Functional & Workflow Specification  
**Product:** AI-Assisted Story Creation Studio  
**Project:** Character-Consistent Long-Range Story Generation  
**Primary Role:** Business Analyst / Product Manager  
**Status:** Draft v1  
**Parent Document:** PROD-01 — Product Requirements Specification

# **1. Purpose**

Tài liệu này đặc tả các workflow chức năng chính của sản phẩm từ góc nhìn người dùng và hệ thống.

Mỗi workflow xác định:

- actor;

- mục tiêu;

- precondition;

- trigger;

- main flow;

- alternate flow;

- exception flow;

- postcondition;

- business rules;

- acceptance criteria;

- dependency.

Tài liệu không mô tả chi tiết:

- bố cục giao diện;

- API endpoint;

- database schema;

- AI model implementation;

- background job architecture.

Những nội dung đó được đặc tả trong các nhóm tài liệu UX, Architecture, Data, API và AI.

# **2. Workflow Overview**

Các workflow chính của sản phẩm gồm:

WF-01 Create Project

WF-02 Initialize Story

WF-03 Manage Story Core

WF-04 Create Character

WF-05 Manage Character Identity & Appearance

WF-06 Manage Character References

WF-07 Create and Manage Scenes

WF-08 Assign Characters to Scene

WF-09 AI-Assisted Scene Generation

WF-10 Generate Scene Image

WF-11 Review Generated Candidates

WF-12 Select Candidate

WF-13 Regenerate Output

WF-14 Handle Canonical Data Changes

WF-15 Review Generation History

WF-16 Complete / Archive Project

# **3. Common Workflow Rules**

## **WF-BR-01**

Mọi operation thay đổi canonical state phải được thực hiện trên một Project hợp lệ.

## **WF-BR-02**

AI-generated proposal không tự động trở thành canonical data nếu workflow yêu cầu review.

## **WF-BR-03**

Generated candidate không được overwrite trực tiếp candidate cũ.

## **WF-BR-04**

Character identity phải được xác định bằng internal identifier, không dựa duy nhất vào character name.

## **WF-BR-05**

Scene generation chỉ được sử dụng context thuộc đúng Project và Story hiện tại.

## **WF-BR-06**

AI failure không được làm mất canonical data đã lưu.

## **WF-BR-07**

Nếu user thay đổi dữ liệu upstream, system phải có khả năng xác định output downstream có thể outdated.

# **4. WF-01 — Create Project**

## **Goal**

Cho phép user tạo workspace mới cho một story.

## **Primary Actor**

Story Creator.

## **Preconditions**

- User đã truy cập application.

- User có quyền tạo Project.

## **Trigger**

User chọn hành động tạo Project mới.

## **Main Flow**

1.  User chọn Create Project.

2.  System yêu cầu thông tin cơ bản.

3.  User cung cấp tối thiểu project name.

4.  User có thể chọn:

    - Blank Project;

    - Start from Story Idea.

5.  System validate input.

6.  System tạo Project.

7.  System gán unique Project ID.

8.  System tạo Story workspace tương ứng.

9.  System đưa user vào Project workspace.

## **Alternate Flow — Start from Story Idea**

1.  User nhập story idea ban đầu.

2.  System lưu input này như source input.

3.  User có thể yêu cầu AI hỗ trợ khởi tạo story.

4.  Workflow tiếp tục sang WF-02.

## **Exception Flow**

Nếu Project không thể tạo:

- không được tạo partial canonical state;

- system thông báo lỗi;

- user có thể retry.

## **Postconditions**

- Project tồn tại.

- Story workspace tồn tại.

- Project ở trạng thái DRAFT hoặc ACTIVE.

## **Acceptance Criteria**

### **AC-WF01-01**

Given user nhập project name hợp lệ,  
when user xác nhận tạo Project,  
then Project phải có unique identifier.

### **AC-WF01-02**

Given việc tạo Project thất bại,  
then không được xuất hiện Project không hoàn chỉnh trong danh sách user.

# **5. WF-02 — Initialize Story**

## **Goal**

Khởi tạo nội dung story từ blank input hoặc story idea.

## **Preconditions**

- Project đã tồn tại.

## **Trigger**

User bắt đầu xây dựng Story.

## **Main Flow — Manual**

1.  User mở Story section.

2.  User nhập các thông tin mong muốn:

    - title;

    - premise;

    - synopsis;

    - tone;

    - setting.

3.  User lưu thông tin.

4.  System cập nhật canonical Story.

## **Main Flow — AI Assisted**

1.  User nhập story idea.

2.  User chọn AI assistance.

3.  System tạo AI generation request.

4.  AI đề xuất:

    - title;

    - premise;

    - synopsis;

    - structure;

    - characters.

5.  System hiển thị proposal.

6.  User:

    - accept;

    - edit;

    - reject.

7.  Chỉ dữ liệu được user xác nhận mới trở thành canonical Story.

## **Alternate Flow**

User chỉ accept một phần proposal.

System phải cho phép giữ những phần được chọn và bỏ các phần còn lại.

## **Postconditions**

Story có ít nhất một phần canonical content.

## **Acceptance Criteria**

### **AC-WF02-01**

AI proposal không được tự động ghi đè Story hiện tại.

### **AC-WF02-02**

User phải có thể sửa proposal trước khi xác nhận.

# **6. WF-03 — Manage Story Core**

## **Goal**

Cho phép user quản lý những dữ liệu được xem là nguồn sự thật của Story.

## **Story Core có thể bao gồm**

- premise;

- world facts;

- important story facts;

- relationships;

- character definitions;

- visual style;

- locked attributes.

## **Main Flow**

1.  User mở Story Core.

2.  System hiển thị canonical information hiện tại.

3.  User chỉnh sửa một hoặc nhiều field.

4.  System validate dữ liệu.

5.  System lưu version mới.

6.  System đánh dấu dependency liên quan nếu thay đổi có ảnh hưởng tới output đã generation.

## **Alternate Flow — Lock Field**

1.  User chọn một field.

2.  User đánh dấu field là locked.

3.  System lưu trạng thái lock.

4.  AI operations sau đó không được tự động thay đổi field đó.

## **Exception Flow**

Nếu update xung đột với dữ liệu bắt buộc:

- system không commit;

- system hiển thị conflict cho user.

## **Acceptance Criteria**

### **AC-WF03-01**

Locked canonical field không được AI tự động ghi đè.

### **AC-WF03-02**

Canonical change phải có version hoặc audit information phù hợp.

# **7. WF-04 — Create Character**

## **Goal**

Tạo recurring character cho Story.

## **Preconditions**

- Project tồn tại.

## **Main Flow — Manual**

1.  User chọn Add Character.

2.  User nhập:

    - name;

    - role;

    - description;

    - visual identity;

    - personality hoặc thông tin tùy chọn.

3.  System validate.

4.  System tạo Character ID.

5.  Character được thêm vào Story Core.

## **Main Flow — AI Assisted**

1.  User chọn tạo character bằng AI.

2.  System cung cấp relevant story context.

3.  AI sinh character proposal.

4.  User review.

5.  User chỉnh sửa nếu cần.

6.  User xác nhận.

7.  System tạo canonical Character.

## **Business Rules**

### **WF04-BR-01**

Character name không phải primary technical identity.

### **WF04-BR-02**

Hai character có thể có tên giống nhau nhưng phải có identifier riêng.

### **WF04-BR-03**

AI-generated character chỉ trở thành canonical sau khi workflow chấp nhận.

## **Acceptance Criteria**

### **AC-WF04-01**

Mỗi character phải có unique Character ID.

### **AC-WF04-02**

User phải có thể chỉnh sửa character sau khi tạo.

# **8. WF-05 — Manage Character Identity & Appearance**

## **Goal**

Phân biệt phần character cần ổn định với phần có thể thay đổi theo story.

## **Character Data Groups**

### **Stable Identity**

Ví dụ:

- facial structure;

- core hair characteristics;

- identity-defining visual traits;

- immutable biological characteristics nếu story yêu cầu.

### **Mutable Appearance**

Ví dụ:

- clothing;

- hairstyle variation được cho phép;

- expression;

- injury;

- temporary accessory;

- age/state variation nếu story quy định.

## **Main Flow**

1.  User mở Character profile.

2.  System hiển thị:

    - Identity;

    - Default Appearance;

    - Current / Scene-dependent State.

3.  User chỉnh sửa.

4.  User có thể lock identity attributes.

5.  System lưu version mới.

## **Business Rules**

### **WF05-BR-01**

Mutable state không được mặc định coi là identity.

### **WF05-BR-02**

Character appearance có thể thay đổi mà không tạo Character mới.

### **WF05-BR-03**

Thay đổi identity quan trọng phải trigger dependency analysis.

## **Acceptance Criteria**

### **AC-WF05-01**

User phải phân biệt được thuộc tính ổn định và thuộc tính scene-dependent.

### **AC-WF05-02**

Changing clothing không được tự động thay đổi character identity.

# **9. WF-06 — Manage Character References**

## **Goal**

Cho phép user quản lý visual reference dùng để giữ character consistency.

## **Preconditions**

- Character tồn tại.

## **Main Flow**

1.  User mở Character References.

2.  User upload hoặc chọn image reference.

3.  System lưu image như asset.

4.  User chọn reference làm canonical.

5.  System liên kết reference với Character.

6.  AI operations sau đó có thể sử dụng reference theo AI specification.

## **Alternate Flow — Generated Reference**

1.  User yêu cầu AI tạo character reference.

2.  System generation một hoặc nhiều candidates.

3.  User review.

4.  User chọn một candidate.

5.  Candidate được gắn làm canonical reference.

## **Business Rules**

### **WF06-BR-01**

Regeneration không được tự động thay thế canonical reference.

### **WF06-BR-02**

User phải chủ động xác nhận khi đổi canonical reference.

## **Acceptance Criteria**

### **AC-WF06-01**

Character có thể có nhiều reference assets.

### **AC-WF06-02**

Tại một thời điểm phải xác định được reference nào đang là canonical nếu tính năng đó được sử dụng.

# **10. WF-07 — Create and Manage Scenes**

## **Goal**

Quản lý cấu trúc story theo các scene có thứ tự.

## **Main Flow**

1.  User mở Scene List.

2.  User chọn Add Scene.

3.  User nhập hoặc generate:

    - title;

    - summary;

    - setting;

    - action;

    - emotion;

    - narrative intent;

    - visual description.

4.  System tạo Scene ID.

5.  Scene được thêm vào sequence.

## **Scene Operations**

User có thể:

- create;

- edit;

- duplicate;

- delete;

- reorder.

## **Business Rules**

### **WF07-BR-01**

Mỗi Scene phải thuộc đúng một Story.

### **WF07-BR-02**

Scene order phải có thể xác định rõ.

### **WF07-BR-03**

Delete Scene không được âm thầm xóa shared assets nếu asset còn được tham chiếu.

## **Acceptance Criteria**

### **AC-WF07-01**

User phải có thể thay đổi thứ tự scene.

### **AC-WF07-02**

Scene reorder không được làm thay đổi Character ID hoặc generation history.

# **11. WF-08 — Assign Characters to Scene**

## **Goal**

Xác định recurring characters nào thực sự xuất hiện trong scene.

## **Main Flow**

1.  User mở Scene.

2.  User chọn Characters.

3.  System hiển thị characters thuộc Story.

4.  User chọn một hoặc nhiều characters.

5.  User có thể đặt scene-specific state cho từng character.

6.  System lưu Scene Character Assignment.

## **Scene-Specific State có thể gồm**

- clothing;

- emotion;

- action;

- position;

- temporary condition;

- scene-specific appearance notes.

## **Business Rules**

### **WF08-BR-01**

Chỉ character được assign mới mặc định được xem là active character trong generation context.

### **WF08-BR-02**

Scene-specific state không được tự động ghi đè canonical identity.

## **Acceptance Criteria**

### **AC-WF08-01**

System phải biết chính xác active characters của từng scene.

### **AC-WF08-02**

Character không xuất hiện trong scene không được vô tình trở thành required generation subject.

# **12. WF-09 — AI-Assisted Scene Generation**

## **Goal**

Cho phép AI hỗ trợ tạo hoặc mở rộng scene.

## **Preconditions**

- Story tồn tại.

- Relevant context có thể được truy xuất.

## **Main Flow**

1.  User chọn Generate hoặc Expand Scene.

2.  User có thể thêm instruction.

3.  System chuẩn bị context.

4.  AI sinh proposal.

5.  Proposal được hiển thị riêng với canonical content.

6.  User:

    - accept all;

    - accept partially;

    - edit;

    - reject.

7.  Accepted fields được cập nhật vào Scene.

## **Exception Flow**

Nếu AI generation thất bại:

- Scene hiện tại không bị thay đổi;

- user có thể retry.

## **Acceptance Criteria**

### **AC-WF09-01**

Failed generation không làm mất nội dung Scene hiện tại.

### **AC-WF09-02**

User phải được review generated scene content trước canonical update nếu workflow yêu cầu confirmation.

# **13. WF-10 — Generate Scene Image**

## **Goal**

Sinh hình ảnh cho Scene bằng context hiện tại.

## **Preconditions**

- Scene tồn tại.

- Scene có dữ liệu tối thiểu để generation.

- Required character context tồn tại.

## **Trigger**

User chọn Generate Image.

## **Main Flow**

1.  System nhận generation request.

2.  System xác định Scene target.

3.  System xác định active characters.

4.  System xây generation context.

5.  System snapshot context.

6.  Generation Job được tạo.

7.  System gọi image-generation pipeline.

8.  Một hoặc nhiều image candidates được sinh.

9.  Candidates được validation.

10. Valid candidates được hiển thị.

11. Workflow chuyển sang WF-11.

## **User-visible States**

- Queued;

- Preparing;

- Generating;

- Validating;

- Completed;

- Failed.

## **Exception Flow — Missing Data**

Nếu scene thiếu required data:

1.  System không generation.

2.  System chỉ rõ dữ liệu còn thiếu.

3.  User bổ sung.

4.  User thử lại.

## **Exception Flow — Generation Failure**

1.  Job thất bại.

2.  Existing selected output vẫn được giữ.

3.  User có thể retry.

## **Acceptance Criteria**

### **AC-WF10-01**

Generation job không được làm UI mất selected image hiện tại.

### **AC-WF10-02**

Mỗi generated candidate phải liên kết tới đúng Scene.

### **AC-WF10-03**

Generation phải sử dụng context snapshot để tránh thay đổi input giữa job.

# **14. WF-11 — Review Generated Candidates**

## **Goal**

Cho phép user đánh giá output trước khi lựa chọn.

## **Main Flow**

1.  System hiển thị generated candidates.

2.  Với mỗi candidate, user có thể xem:

    - preview;

    - generation status;

    - warning nếu có;

    - generation time/version khi phù hợp.

3.  User so sánh candidates.

4.  User quyết định:

    - Select;

    - Regenerate;

    - Keep without selecting;

    - Discard/archive candidate.

## **Alternate Flow — Consistency Warning**

1.  Candidate có validation warning.

2.  System hiển thị warning.

3.  User vẫn có thể review.

4.  Nếu warning không blocking, user có thể lựa chọn candidate.

## **Acceptance Criteria**

### **AC-WF11-01**

User phải phân biệt được candidate nào đang selected.

### **AC-WF11-02**

Candidate có warning phải thể hiện trạng thái rõ ràng.

# **15. WF-12 — Select Candidate**

## **Goal**

Chọn một generated candidate làm output chính hiện tại của Scene.

## **Preconditions**

- Candidate tồn tại.

- Candidate không bị trạng thái blocking invalid.

## **Main Flow**

1.  User chọn Candidate.

2.  User chọn Use this result.

3.  System validate candidate eligibility.

4.  System cập nhật Scene selected output reference.

5.  Candidate trở thành SELECTED.

6.  Candidate trước đó nếu có không bị xóa.

## **Business Rules**

### **WF12-BR-01**

Scene chỉ có một selected image chính tại một thời điểm cho cùng output slot.

### **WF12-BR-02**

Previous candidate vẫn tồn tại trong history.

## **Acceptance Criteria**

### **AC-WF12-01**

Selecting candidate B không được xóa candidate A.

### **AC-WF12-02**

User phải có thể quay lại candidate trước đó nếu candidate đó còn hợp lệ.

# **16. WF-13 — Regenerate Output**

## **Goal**

Tạo output mới mà không mất generation history.

## **Regeneration Modes**

### **Mode A — Same Input**

Giữ nguyên context và instruction chính.

### **Mode B — Modified Instruction**

User bổ sung yêu cầu.

Ví dụ:

- make the character look more worried;

- use a wider shot;

- show both characters clearly.

### **Mode C — Updated Canonical Context**

Generation sử dụng phiên bản Story/Character/Scene mới nhất.

## **Main Flow**

1.  User chọn Regenerate.

2.  User chọn hoặc nhập instruction.

3.  System tạo generation request mới.

4.  New Generation Attempt được tạo.

5.  Candidate mới được sinh.

6.  Candidate cũ vẫn tồn tại.

7.  User review candidate mới.

## **Business Rules**

### **WF13-BR-01**

Regeneration khác Retry.

### **WF13-BR-02**

User-requested regeneration luôn phải tạo attempt mới.

## **Acceptance Criteria**

### **AC-WF13-01**

Regeneration không overwrite output cũ.

### **AC-WF13-02**

Generation history phải thể hiện được thứ tự các attempts.

# **17. WF-14 — Handle Canonical Data Changes**

## **Goal**

Xử lý tình huống user chỉnh sửa dữ liệu đã từng được sử dụng để generation.

## **Example**

User thay đổi hair color của Character A.

Character A đã xuất hiện trong Scene 01, 03 và 08.

## **Main Flow**

1.  User cập nhật canonical information.

2.  System lưu version mới.

3.  System xác định affected dependencies.

4.  Existing generations vẫn được giữ.

5.  Những output liên quan có thể được đánh dấu:

    - OUTDATED_CONTEXT;

    - hoặc trạng thái tương đương.

6.  User được thông báo.

7.  User có thể:

    - giữ output hiện tại;

    - regenerate scene cụ thể;

    - regenerate nhiều affected scenes nếu chức năng hỗ trợ.

## **Business Rules**

### **WF14-BR-01**

Canonical update không được tự động xóa output cũ.

### **WF14-BR-02**

System không bắt buộc tự động regenerate toàn bộ story.

### **WF14-BR-03**

User là người quyết định có regenerate hay không, trừ workflow kỹ thuật đặc biệt được định nghĩa khác.

## **Acceptance Criteria**

### **AC-WF14-01**

System phải nhận biết output được sinh từ context version cũ nếu metadata cho phép.

### **AC-WF14-02**

User phải được biết output nào có khả năng outdated.

# **18. WF-15 — Review Generation History**

## **Goal**

Cho phép user kiểm tra các lần generation trước đây.

## **Main Flow**

1.  User mở history của Scene hoặc target.

2.  System hiển thị attempts theo thời gian.

3.  Mỗi attempt có thể hiển thị:

    - candidate;

    - status;

    - timestamp;

    - instruction;

    - selected state;

    - warning.

4.  User có thể xem lại candidate.

5.  User có thể chọn lại candidate hợp lệ trước đó.

## **Acceptance Criteria**

### **AC-WF15-01**

History không bị mất khi regenerate.

### **AC-WF15-02**

User phải xác định được candidate hiện tại xuất phát từ generation attempt nào.

# **19. WF-16 — Complete / Archive Project**

## **Goal**

Cho phép user kết thúc vòng đời active của Project.

## **Complete Flow**

1.  User chọn Mark as Completed.

2.  System validate Project state.

3.  Project chuyển sang COMPLETED.

4.  User vẫn có thể xem nội dung.

## **Archive Flow**

1.  User chọn Archive.

2.  Project chuyển sang ARCHIVED.

3.  Project không còn xuất hiện trong active list mặc định.

4.  Dữ liệu không bị xóa.

## **Business Rules**

### **WF16-BR-01**

Archive khác Delete.

### **WF16-BR-02**

Completed Project có thể được reopen nếu product policy cho phép.

# **20. Cross-Workflow: AI Failure Handling**

AI operations có thể thất bại ở nhiều workflow.

Quy tắc chung:

AI Request

↓

Generation Failed

↓

Preserve Existing State

↓

Show Failure

↓

Retry / Modify / Cancel

Giải thích bằng lời:

Nếu AI lỗi, hệ thống giữ nguyên dữ liệu đã tồn tại. User được thông báo lỗi và có thể thử lại, thay đổi input hoặc hủy operation. Không workflow nào được phép coi AI failure là lý do để phá hỏng canonical Story.

# **21. Cross-Workflow: Unsaved Changes**

Nếu user đang chỉnh sửa canonical data nhưng chưa lưu và bắt đầu generation:

System phải áp dụng một policy rõ ràng, ví dụ:

- yêu cầu lưu trước;

- hoặc generation từ explicit draft snapshot.

System không được ngầm sử dụng một trạng thái không xác định.

# **22. Cross-Workflow: Concurrent Change**

Nếu generation đang chạy trong khi user thay đổi Scene hoặc Character:

1.  Running job tiếp tục sử dụng snapshot ban đầu.

2.  New canonical data được lưu độc lập.

3.  Khi generation hoàn thành, system so sánh version.

4.  Candidate có thể được đánh dấu outdated.

Điều này tránh việc một job đang chạy bị thay đổi input giữa chừng.

# **23. Functional Permissions**

Trong MVP với single creator, permission model có thể đơn giản.

Story Creator được phép:

- create;

- edit;

- generate;

- regenerate;

- select;

- archive.

Administrator chỉ quản lý system configuration.

Collaborative roles như:

- editor;

- reviewer;

- viewer;

không thuộc MVP bắt buộc.

# **24. Workflow Priority for MVP**

## **Priority P0 — Must Have**

- WF-01 Create Project

- WF-02 Initialize Story

- WF-04 Create Character

- WF-05 Manage Character

- WF-06 Character Reference

- WF-07 Scene Management

- WF-08 Scene Character Assignment

- WF-10 Generate Image

- WF-11 Review Candidate

- WF-12 Select Candidate

- WF-13 Regenerate

- WF-15 Generation History

## **Priority P1 — Important**

- WF-03 Manage Story Core

- WF-09 AI-Assisted Scene Generation

- WF-14 Handle Canonical Changes

## **Priority P2 — Enhancement**

- advanced archive/completion workflow;

- batch regeneration;

- advanced dependency review.

# **25. End-to-End Primary Scenario**

Một complete happy path của sản phẩm:

User creates Project

↓

Adds Story Idea

↓

AI proposes Story

↓

User confirms Story

↓

Creates Characters

↓

Selects Character References

↓

Creates Scene Sequence

↓

Assigns Characters to Scene

↓

Generates Image

↓

Reviews Candidates

↓

Selects Best Candidate

↓

Continues to Next Scene

↓

Regenerates when necessary

↓

Completes Story

Giải thích bằng lời:

Người dùng bắt đầu từ một ý tưởng, xây dựng story và nhân vật trước. Sau đó story được chia thành các scene. Mỗi scene biết nhân vật nào tham gia và trạng thái của họ trong cảnh đó. Khi user yêu cầu sinh ảnh, hệ thống tự chuẩn bị context, chạy AI và trả về candidates. User review, chọn hoặc regenerate. Chu trình này lặp lại cho đến khi story hoàn chỉnh.

# **26. End-to-End Consistency Scenario**

Ví dụ:

Character A xuất hiện ở Scene 01.

Sau đó A không xuất hiện trong Scene 02–09.

A xuất hiện lại ở Scene 10.

Workflow mong muốn:

Scene 01

Character A established

↓

Scene 02–09

A absent

↓

Scene 10

A assigned again

↓

System retrieves A canonical identity

↓

Generation Context prepared

↓

Image generated with A identity

Giải thích bằng lời:

Việc character không xuất hiện trong nhiều scene không làm mất identity của character. Khi A quay trở lại, Scene 10 tham chiếu tới chính Character A đã tồn tại, và AI layer sử dụng identity context tương ứng.

# **27. End-to-End Multi-Character Scenario**

Scene 12 gồm:

- Character A;

- Character B.

Workflow:

1.  User assign A và B vào Scene.

2.  User đặt state riêng cho từng character.

3.  System chuẩn bị context riêng cho A và B.

4.  AI generation chạy.

5.  Validation kiểm tra:

    - A có xuất hiện;

    - B có xuất hiện;

    - identity A và B không bị trộn;

    - scene intent được giữ.

6.  Candidate được trả về.

7.  User review.

# **28. Dependency Mapping**

| **Workflow** | **Depends On**                  |
|--------------|---------------------------------|
| WF-01        | Project Management              |
| WF-02        | AI Generation, Story Data       |
| WF-03        | Story Core, Versioning          |
| WF-04        | Story Core                      |
| WF-05        | Character Model                 |
| WF-06        | Asset Storage, Generation       |
| WF-07        | Scene Data                      |
| WF-08        | Character + Scene               |
| WF-09        | AI-01, AI-02                    |
| WF-10        | AI-01, AI-02, AI-03             |
| WF-11        | AI-03, UX                       |
| WF-12        | Candidate Management            |
| WF-13        | AI-03                           |
| WF-14        | Versioning, Dependency Tracking |
| WF-15        | Generation History              |
| WF-16        | Project Lifecycle               |

# **29. Traceability to PROD-01**

| **PROD-02 Workflow** | **PROD-01 Requirement Area**      |
|----------------------|-----------------------------------|
| WF-01                | Project Management                |
| WF-02                | Story Creation                    |
| WF-03                | Story Continuity / Canonical Data |
| WF-04                | Character Management              |
| WF-05                | Identity vs Appearance            |
| WF-06                | Character Reference               |
| WF-07                | Scene Management                  |
| WF-08                | Scene Character Assignment        |
| WF-09                | AI Assistance                     |
| WF-10                | Image Generation                  |
| WF-11                | Review Experience                 |
| WF-12                | Candidate Selection               |
| WF-13                | Regeneration                      |
| WF-14                | Dependency Changes                |
| WF-15                | Generation History                |
| WF-16                | Project Lifecycle                 |

# **30. Product Workflow Principle**

Nguyên tắc chung của mọi workflow là:

> **Canonical data is edited deliberately; AI output is reviewed before it becomes part of the story state.**

Ứng dụng phải luôn giữ sự phân biệt giữa:

What the story currently IS

và

What AI is suggesting or generating.

Nhờ đó user giữ quyền kiểm soát story, trong khi AI vẫn có thể hỗ trợ mạnh trong quá trình sáng tác.
