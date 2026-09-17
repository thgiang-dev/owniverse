# **PROD-01 — Product Requirements Specification**

**Document ID:** PROD-01  
**Document Type:** Product Requirements Specification  
**Product:** AI-Assisted Story Creation Studio  
**Project:** Character-Consistent Long-Range Story Generation  
**Primary Roles:** Product Manager / Business Analyst  
**Status:** Draft v1  
**Related Documents:** PROD-02, UX-01, ARCH-01, AI-01, AI-02, AI-03

# **1. Purpose**

Tài liệu này xác định yêu cầu sản phẩm cấp cao cho một nền tảng hỗ trợ người dùng xây dựng câu chuyện và tạo chuỗi hình ảnh bằng AI.

Sản phẩm hướng tới việc giúp người dùng chuyển một ý tưởng ban đầu thành một story có cấu trúc, quản lý nhân vật và scene, sau đó sử dụng AI để tạo nội dung và hình ảnh trong khi duy trì tính nhất quán xuyên suốt câu chuyện.

Tài liệu trả lời các câu hỏi:

- Sản phẩm giải quyết vấn đề gì?

- Đối tượng sử dụng là ai?

- Người dùng có thể làm được những gì?

- Dữ liệu nào được xem là thông tin chính thức của story?

- AI đóng vai trò gì trong sản phẩm?

- Phạm vi nào thuộc phiên bản luận văn?

- Điều kiện nào xác định sản phẩm đáp ứng yêu cầu?

# **2. Product Vision**

Xây dựng một **AI-assisted creative studio** nơi người dùng có thể:

1.  tạo một story từ ý tưởng;

2.  xây dựng và quản lý nhân vật;

3.  chia story thành nhiều scene;

4.  sử dụng AI để hỗ trợ phát triển nội dung;

5.  tạo hình ảnh cho từng scene;

6.  duy trì character consistency xuyên suốt story;

7.  review, chỉnh sửa và regenerate kết quả;

8.  quản lý toàn bộ quá trình sáng tác trong một project thống nhất.

Sản phẩm không nhằm thay thế hoàn toàn người sáng tạo.

AI đóng vai trò **creative assistant**, trong khi người dùng vẫn kiểm soát story và quyết định kết quả cuối cùng.

# **3. Problem Statement**

Các hệ thống generative AI hiện nay có thể tạo nội dung hoặc hình ảnh chất lượng cao từ prompt đơn lẻ.

Tuy nhiên, khi xây dựng một câu chuyện gồm nhiều scene, xuất hiện các vấn đề:

- người dùng phải tự quản lý lượng context ngày càng lớn;

- thông tin về nhân vật dễ bị thay đổi giữa các scene;

- nhân vật có thể mất đặc điểm nhận dạng;

- nhiều nhân vật có thể bị trộn đặc điểm với nhau;

- trạng thái câu chuyện dễ trở nên mâu thuẫn;

- việc regenerate một scene có thể làm mất những thông tin quan trọng;

- người dùng phải tự viết lại prompt rất nhiều lần;

- khó quản lý nhiều version của cùng một scene hoặc image;

- các công cụ generation thường tập trung vào từng output riêng lẻ thay vì toàn bộ story.

Sản phẩm cần cung cấp một **story-centric workflow**, trong đó AI generation luôn hoạt động dựa trên dữ liệu có cấu trúc của cả câu chuyện.

# **4. Product Goals**

## **PG-01 — Structured Story Creation**

Cho phép người dùng xây dựng story bằng cấu trúc rõ ràng thay vì chỉ sử dụng một chuỗi prompt độc lập.

## **PG-02 — Character Consistency**

Hỗ trợ duy trì danh tính và đặc điểm nhận dạng của recurring characters xuyên suốt nhiều scene.

## **PG-03 — Long-Range Story Support**

Cho phép một character xuất hiện, biến mất trong nhiều scene và xuất hiện trở lại mà hệ thống vẫn có khả năng sử dụng đúng identity của nhân vật.

## **PG-04 — Multi-Character Support**

Hỗ trợ story có nhiều recurring characters và hạn chế việc đặc điểm của character này bị trộn sang character khác.

## **PG-05 — Human Creative Control**

Người dùng phải có khả năng:

- chỉnh sửa;

- regenerate;

- chọn candidate;

- khóa thông tin quan trọng;

- override đề xuất của AI.

## **PG-06 — Traceable AI Generation**

Kết quả AI phải có lịch sử generation và version rõ ràng để người dùng có thể quay lại hoặc so sánh.

## **PG-07 — Integrated Creative Workflow**

Story planning, character management, scene management và image generation phải nằm trong cùng một workflow thay vì hoạt động như các công cụ tách rời.

# **5. Non-Goals**

Phiên bản sản phẩm trong phạm vi luận văn không đặt mục tiêu:

- xây dựng nền tảng xuất bản truyện thương mại hoàn chỉnh;

- cạnh tranh trực tiếp với phần mềm minh họa chuyên nghiệp;

- cung cấp full-feature image editor;

- tạo animation hoặc video hoàn chỉnh;

- tự động viết toàn bộ tiểu thuyết dài mà không có sự tham gia của người dùng;

- duy trì một character identity chung giữa mọi project của user;

- hỗ trợ collaborative editing quy mô lớn;

- xây dựng marketplace;

- xây dựng social network cho creator;

- hỗ trợ mọi loại generative model hiện có.

# **6. Target Users**

## **Actor A01 — Story Creator**

Người dùng chính của hệ thống.

Có thể là:

- người viết truyện;

- sinh viên;

- content creator;

- comic creator;

- storyboard creator;

- người muốn thử nghiệm ý tưởng bằng AI.

Không yêu cầu người dùng phải có kiến thức về diffusion model hoặc prompt engineering chuyên sâu.

## **Actor A02 — System Administrator**

Quản lý các cấu hình vận hành cần thiết như:

- AI provider;

- model availability;

- generation configuration;

- system limits.

Actor này không tham gia trực tiếp vào quá trình sáng tác story.

## **Actor A03 — AI Services**

Các model và AI services được hệ thống sử dụng để:

- phân tích story;

- hỗ trợ viết nội dung;

- chuẩn hóa dữ liệu;

- tạo prompt;

- sinh ảnh;

- hỗ trợ validation.

AI Service là system actor, không phải end user.

# **7. Core Product Concepts**

## **7.1 Project**

Workspace cao nhất cho một tác phẩm.

Một Project chứa:

- story;

- characters;

- scenes;

- generated assets;

- generation history;

- configuration.

# **7.2 Story**

Nội dung trung tâm của Project.

Story chứa thông tin tổng thể như:

- title;

- premise;

- synopsis;

- tone;

- setting;

- narrative information.

# **7.3 Story Core / Story Bible**

Nguồn dữ liệu có cấu trúc chứa những thông tin canonical quan trọng mà hệ thống cần duy trì nhất quán.

Có thể bao gồm:

- character definitions;

- world facts;

- important story facts;

- relationships;

- style information;

- locked attributes.

Story Core không phải một prompt lớn.

Nó là **nguồn dữ liệu chuẩn của story** mà các AI operation sử dụng để tạo context phù hợp.

# **7.4 Character**

Một entity đại diện cho nhân vật.

Character có thể chứa:

- identity;

- visual identity;

- role;

- personality;

- default appearance;

- mutable appearance state;

- reference assets.

# **7.5 Scene**

Đơn vị kể chuyện và generation chính.

Một Scene mô tả:

- thời điểm hoặc vị trí trong story;

- setting;

- characters xuất hiện;

- action;

- emotion;

- visual information;

- generated outputs.

# **7.6 Candidate**

Một kết quả AI được tạo ra nhưng chưa nhất thiết là kết quả được user chọn.

Ví dụ một scene có thể có bốn generated images nhưng chỉ một image được chọn.

# **7.7 Selected Output**

Candidate hiện được lựa chọn làm output chính của target tương ứng.

# **8. Core User Journey**

Luồng sản phẩm tổng quát:

Create Project

↓

Provide Story Idea

↓

Create / Generate Story Structure

↓

Create Characters

↓

Review Story Core

↓

Create Scenes

↓

Review / Edit Scene

↓

Generate Scene Image

↓

Review Candidates

↓

Select / Regenerate

↓

Continue Story

↓

Review Completed Story

Luồng này có nghĩa là người dùng không chỉ nhập một prompt rồi nhận ảnh.

Họ dần xây dựng một story có trạng thái và cấu trúc rõ ràng. AI hỗ trợ từng bước nhưng dữ liệu story vẫn thuộc quyền kiểm soát của application và người dùng.

# **9. Functional Capability Groups**

## **PC-01 — Project Management**

Người dùng phải có khả năng:

- tạo project;

- mở project;

- chỉnh sửa metadata cơ bản;

- xem danh sách project;

- xóa hoặc archive project.

# **10. Story Creation**

## **PR-FR-01**

System SHALL cho phép user tạo một story từ:

- blank project;

- story idea;

- synopsis hoặc mô tả có sẵn.

## **PR-FR-02**

System SHALL cho phép AI đề xuất story structure từ user input.

## **PR-FR-03**

User SHALL có khả năng chỉnh sửa AI-generated story information.

## **PR-FR-04**

System SHALL phân biệt thông tin canonical của story với AI-generated proposal chưa được xác nhận.

# **11. Character Management**

## **PR-FR-05**

System SHALL cho phép user tạo character thủ công.

## **PR-FR-06**

System SHALL cho phép AI hỗ trợ tạo character từ story context.

## **PR-FR-07**

User SHALL có khả năng chỉnh sửa character profile.

## **PR-FR-08**

System SHALL duy trì unique identity cho từng recurring character trong phạm vi một story.

## **PR-FR-09**

System SHALL cho phép character có:

- stable identity attributes;

- mutable appearance/state attributes.

## **PR-FR-10**

User SHALL có khả năng khóa những character attributes không được AI tự ý thay đổi.

# **12. Character Reference**

## **PR-FR-11**

System SHALL cho phép gắn visual reference với character.

## **PR-FR-12**

System SHALL lưu reference được user lựa chọn làm canonical reference.

## **PR-FR-13**

Regeneration SHALL không tự động thay thế canonical character reference.

# **13. Scene Management**

## **PR-FR-14**

System SHALL cho phép story chứa nhiều scene có thứ tự.

## **PR-FR-15**

User SHALL có khả năng:

- tạo scene;

- sửa scene;

- xóa scene;

- reorder scene.

## **PR-FR-16**

Một Scene SHALL xác định những character nào xuất hiện trong scene đó.

## **PR-FR-17**

Scene SHALL có thể lưu:

- setting;

- action;

- emotion;

- narrative intent;

- visual description.

## **PR-FR-18**

System SHALL cho phép AI hỗ trợ tạo hoặc mở rộng scene.

# **14. Story Continuity**

## **PR-FR-19**

System SHALL sử dụng dữ liệu story hiện tại khi thực hiện generation cho một scene.

## **PR-FR-20**

AI generation SHALL ưu tiên canonical information hơn AI assumptions.

## **PR-FR-21**

AI SHALL không tự ý thay đổi locked story facts.

## **PR-FR-22**

System SHALL có khả năng phát hiện output có dấu hiệu xung đột với canonical context.

# **15. Image Generation**

## **PR-FR-23**

User SHALL có khả năng yêu cầu tạo image cho một scene.

## **PR-FR-24**

System SHALL tự động chuẩn bị generation context dựa trên:

- scene;

- relevant characters;

- story information;

- style;

- consistency information.

User không bắt buộc phải tự xây dựng toàn bộ generation prompt.

## **PR-FR-25**

System SHALL hỗ trợ tạo một hoặc nhiều image candidates.

## **PR-FR-26**

User SHALL có khả năng xem các candidate của scene.

## **PR-FR-27**

User SHALL có khả năng lựa chọn một candidate làm selected output.

# **16. Regeneration**

## **PR-FR-28**

User SHALL có khả năng regenerate scene output.

## **PR-FR-29**

Regeneration SHALL tạo candidate mới và không phá hủy candidate cũ.

## **PR-FR-30**

User SHALL có thể thêm instruction khi regenerate.

Ví dụ:

- thay đổi biểu cảm;

- thay đổi góc máy;

- thay đổi pose;

- nhấn mạnh một hành động.

# **17. Generation History**

## **PR-FR-31**

System SHALL lưu history của các generation attempts.

## **PR-FR-32**

User SHALL có khả năng xem lại các output trước đó của cùng một scene.

## **PR-FR-33**

User SHALL có khả năng chuyển selected output từ candidate hiện tại sang một candidate trước đó nếu candidate đó vẫn hợp lệ.

# **18. Style Management**

## **PR-FR-34**

Project SHALL có thể xác định visual style chủ đạo.

## **PR-FR-35**

Style SHALL được sử dụng nhất quán trong image-generation context.

## **PR-FR-36**

Phiên bản luận văn SHOULD cung cấp một tập style được kiểm soát thay vì cố gắng hỗ trợ số lượng style không giới hạn.

Mục tiêu ưu tiên chất lượng và consistency hơn số lượng.

# **19. AI Assistance**

AI có thể hỗ trợ các nhiệm vụ:

- story ideation;

- character creation;

- scene expansion;

- structured extraction;

- prompt generation;

- image generation;

- validation;

- consistency checking.

## **PR-FR-37**

User SHALL luôn có khả năng review hoặc chỉnh sửa những dữ liệu quan trọng do AI đề xuất.

## **PR-FR-38**

AI SHALL NOT trở thành nguồn canonical duy nhất mà không thông qua application workflow.

# **20. Consistency Requirements**

## **PR-FR-39 — Character Identity**

Recurring character SHOULD giữ những đặc điểm nhận dạng cốt lõi xuyên suốt các scene.

## **PR-FR-40 — Long-Range Reappearance**

Character xuất hiện trở lại sau nhiều scene SHOULD vẫn sử dụng đúng identity của character đó.

## **PR-FR-41 — Multi-Character Separation**

Hệ thống SHOULD hạn chế việc đặc điểm nhận dạng của một character xuất hiện trên character khác.

## **PR-FR-42 — Mutable Appearance**

Consistency requirement SHALL NOT buộc mọi visual attribute phải luôn giống nhau.

Character phải được phép thay đổi hợp lý về:

- clothing;

- expression;

- pose;

- action;

- temporary condition.

# **21. Review Experience**

## **PR-FR-43**

System SHALL cung cấp trạng thái rõ ràng cho generation:

- waiting;

- generating;

- validating;

- completed;

- failed.

## **PR-FR-44**

System SHOULD thông báo nếu output có consistency warning.

## **PR-FR-45**

Warning SHALL không tự động ngăn user xem candidate trừ khi output thuộc loại không được phép sử dụng.

# **22. Editing and Dependency Changes**

## **PR-FR-46**

Khi user thay đổi canonical character hoặc story information, system SHALL giữ dữ liệu mới làm source of truth cho generation sau đó.

## **PR-FR-47**

System SHOULD xác định những generated outputs có khả năng được tạo từ context cũ.

## **PR-FR-48**

System SHOULD cho phép user quyết định regenerate output bị ảnh hưởng thay vì tự động regenerate toàn bộ project.

# **23. Persistence Requirements**

## **PR-FR-49**

Project SHALL được lưu để user có thể tiếp tục trong phiên làm việc sau.

## **PR-FR-50**

Canonical story data SHALL được lưu độc lập với generated assets.

## **PR-FR-51**

Generated images SHALL được liên kết với:

- project;

- scene;

- generation attempt;

- candidate.

# **24. Product States**

Một project có thể trải qua:

DRAFT

ACTIVE

COMPLETED

ARCHIVED

Trạng thái này mô tả lifecycle của project, không phải trạng thái generation job.

# **25. User Control Principles**

Người dùng phải giữ quyền quyết định đối với:

- story direction;

- canonical character information;

- scene content;

- selected image;

- regenerate;

- important locked attributes.

AI có thể đề xuất nhưng không được âm thầm thay đổi các quyết định này.

# **26. Product-Level Error Principles**

Nếu AI generation thất bại:

- canonical data không được mất;

- user phải nhận được trạng thái dễ hiểu;

- user phải có thể thử lại;

- failed output không được thay thế successful output hiện tại.

# **27. Non-Functional Product Requirements**

## **PR-NFR-01 — Usability**

Người dùng không cần kiến thức chuyên môn về generative AI để thực hiện workflow chính.

## **PR-NFR-02 — Responsiveness**

Generation chạy lâu không được làm toàn bộ application bị khóa.

## **PR-NFR-03 — Recoverability**

AI failure không được làm mất dữ liệu story đã lưu.

## **PR-NFR-04 — Traceability**

Generated output phải truy ngược được tới scene và generation tương ứng.

## **PR-NFR-05 — Extensibility**

Sản phẩm phải có khả năng thay đổi AI model/provider mà không làm thay đổi toàn bộ product workflow.

## **PR-NFR-06 — Consistency**

Các thành phần application phải sử dụng cùng canonical story state.

# **28. MVP Scope**

Phiên bản MVP phục vụ luận văn phải hỗ trợ tối thiểu:

1.  project creation;

2.  story definition;

3.  character creation và editing;

4.  character reference management;

5.  scene creation và ordering;

6.  scene-character assignment;

7.  AI-assisted story/scene generation;

8.  image generation;

9.  character-aware generation context;

10. candidate management;

11. image selection;

12. regeneration;

13. generation history;

14. basic consistency validation;

15. story/project persistence.

# **29. Future Scope**

Các khả năng có thể phát triển sau luận văn:

- collaborative story editing;

- comic panel layout editor;

- speech bubble editor;

- automatic manga page composition;

- video generation;

- animation;

- voice generation;

- cross-project reusable character library;

- publishing workflow;

- creator marketplace;

- mobile application;

- social/community features.

Những khả năng này không phải dependency của MVP.

# **30. Product Success Criteria**

Sản phẩm được xem là đáp ứng mục tiêu nếu một người dùng có thể:

### **SC-01**

Bắt đầu từ một story idea và tạo được một project có cấu trúc.

### **SC-02**

Xây dựng ít nhất nhiều recurring characters có identity riêng.

### **SC-03**

Tạo story gồm nhiều scene.

### **SC-04**

Sinh image cho từng scene mà không cần tự xây dựng toàn bộ technical prompt.

### **SC-05**

Cho character xuất hiện trở lại ở scene xa hơn và hệ thống vẫn sử dụng đúng character context.

### **SC-06**

Review nhiều generated candidates và chọn output mong muốn.

### **SC-07**

Regenerate mà không mất output cũ.

### **SC-08**

Chỉnh sửa canonical data và sử dụng dữ liệu mới cho generation tiếp theo.

### **SC-09**

Hoàn thành một story sequence từ đầu đến cuối trong cùng application workflow.

# **31. Key Business Rules**

## **BR-01**

Mỗi Project thuộc về một story workspace độc lập.

## **BR-02**

Mỗi Character phải có unique identifier trong Project.

## **BR-03**

Character name không được sử dụng làm identity kỹ thuật duy nhất.

## **BR-04**

Scene phải biết rõ những recurring characters nào xuất hiện.

## **BR-05**

Canonical data có độ ưu tiên cao hơn generated assumptions.

## **BR-06**

Locked canonical attributes không được AI tự động thay đổi.

## **BR-07**

Generated output chưa được chấp nhận không được coi là canonical story state.

## **BR-08**

Regeneration không được xóa generation history trước đó.

## **BR-09**

Selected output phải tham chiếu tới một candidate tồn tại.

## **BR-10**

AI failure không được làm thay đổi canonical state.

# **32. Dependencies**

Sản phẩm phụ thuộc vào:

- AI text-generation service;

- image-generation service;

- persistence layer;

- asset storage;

- background job execution;

- context/memory subsystem;

- validation subsystem.

Việc lựa chọn implementation cụ thể được mô tả trong các tài liệu Architecture và AI.

# **33. Assumptions**

- User sử dụng hệ thống chủ yếu trên desktop/web.

- Generation có thể kéo dài nhiều giây hoặc lâu hơn.

- Một story có thể có nhiều recurring characters.

- Một character có thể không xuất hiện liên tục.

- User có thể chỉnh sửa dữ liệu bất cứ lúc nào.

- AI output không được xem là luôn chính xác.

- Human review vẫn là thành phần quan trọng của creative workflow.

# **34. Out-of-Scope Clarification**

Ứng dụng là **sản phẩm minh họa và vận dụng kết quả nghiên cứu**, không phải bản thân contribution khoa học duy nhất của luận văn.

Việc application hỗ trợ character consistency không đồng nghĩa mọi quy tắc consistency đều được giải quyết ở tầng Product.

Product chỉ định nghĩa **hành vi mà người dùng và hệ thống cần đạt được**.

Cơ chế kỹ thuật được phân bổ cho:

- AI specifications;

- architecture;

- research methodology.

# **35. Traceability Overview**

PROD-01

Product Requirements

↓

PROD-02

Functional & Workflow Specification

↓

UX-01 / UX-02

User Experience & Interface

↓

ARCH / DATA / API

Technical System Design

↓

AI-01 / AI-02 / AI-03

AI Application Behaviour

↓

QA-01 / QA-02

Verification & Acceptance

PROD-01 là nguồn yêu cầu sản phẩm cấp cao.

Các tài liệu phía sau phải giải thích **cách hiện thực các yêu cầu trong PROD-01**, không được âm thầm thay đổi mục tiêu sản phẩm.

# **36. Product Principle**

Nguyên tắc trung tâm của sản phẩm là:

> **The story is the source of truth; AI is an assistant working around it.**

Người dùng không xây dựng một chuỗi prompt rời rạc.

Họ xây dựng **một story có cấu trúc và trạng thái rõ ràng**, còn AI sử dụng story đó để hỗ trợ quá trình sáng tạo, generation và duy trì consistency.
