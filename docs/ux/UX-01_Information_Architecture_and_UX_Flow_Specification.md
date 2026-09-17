# **UX-01 — Information Architecture & UX Flow Specification**

**Document ID:** UX-01  
**Document Type:** UX Architecture Specification  
**Product:** AI-Assisted Story Creation Studio  
**Project:** Character-Consistent Long-Range Story Generation  
**Primary Role:** UX Architect / Product Designer  
**Status:** Draft v1  
**Parent Documents:** PROD-01, PROD-02  
**Related Documents:** UX-02, ARCH-01, AI-01, AI-02, AI-03

# **1. Purpose**

Tài liệu này xác định cấu trúc trải nghiệm người dùng của sản phẩm, bao gồm:

- information architecture;

- navigation model;

- workspace structure;

- screen hierarchy;

- core UX flows;

- trạng thái giao diện;

- cách user di chuyển giữa Story, Character, Scene và Generation;

- cách hệ thống thể hiện AI processing, warning và version changes.

Mục tiêu chính là đảm bảo người dùng có thể xây dựng một story dài và phức tạp mà không bị mất phương hướng hoặc phải hiểu kiến trúc kỹ thuật phía sau.

# **2. UX Goals**

## **UX-G01 — Story-Centric Experience**

Giao diện phải xoay quanh **story**, không xoay quanh AI model hay prompt.

User cần cảm thấy mình đang xây dựng một tác phẩm, không phải vận hành một công cụ kỹ thuật.

## **UX-G02 — Clear Creative Progress**

Người dùng phải dễ dàng biết:

- mình đang ở project nào;

- đang chỉnh scene nào;

- scene nào đã có image;

- scene nào chưa hoàn thành;

- character nào xuất hiện;

- generation nào đang chạy.

## **UX-G03 — Low AI Complexity**

Các khái niệm kỹ thuật như:

- context window;

- seed;

- model routing;

- memory;

- inference;

không được trở thành yêu cầu bắt buộc để user hoàn thành workflow cơ bản.

## **UX-G04 — Human Control**

User phải luôn nhìn thấy và kiểm soát:

- canonical information;

- AI suggestion;

- selected output;

- regeneration;

- locked information.

## **UX-G05 — Fast Iteration**

Creative workflow phải hỗ trợ vòng lặp:

Create

↓

Generate

↓

Review

↓

Adjust

↓

Regenerate

mà không buộc user rời khỏi context hiện tại.

# **3. Primary UX Model**

Sản phẩm sử dụng mô hình:

Project

↓

Story Workspace

├── Story

├── Characters

├── Scenes

└── Assets / Generations

Giải thích bằng lời:

Project là workspace của toàn bộ tác phẩm.

Bên trong Project, user không di chuyển qua các công cụ AI độc lập mà làm việc với Story, Character và Scene. Generation chỉ xuất hiện như một hành động hỗ trợ trên các entity này.

# **4. Global Information Architecture**

Cấu trúc cấp cao:

Home

│

├── Projects

│ ├── Recent Projects

│ └── Archived Projects

│

└── Project Workspace

│

├── Overview

├── Story

├── Characters

├── Scenes

├── Generation History

└── Project Settings

# **5. Home**

Home là entry point chính sau khi user truy cập ứng dụng.

## **Nội dung chính**

- Create New Project;

- Recent Projects;

- Continue Working;

- Archived Projects nếu cần.

## **UX Priority**

Hành động nổi bật nhất phải là:

**Create Project** hoặc **Continue Current Project**.

Không nên đưa model settings hoặc system configuration ra Home.

# **6. Project Workspace**

Project Workspace là màn hình làm việc chính.

Layout tổng quát nên gồm:

┌──────────────────────────────────────────────┐

│ Top Navigation / Project Context │

├──────────────┬───────────────────────────────┤

│ │ │

│ Sidebar │ Main Workspace │

│ │ │

│ Overview │ │

│ Story │ │

│ Characters │ │

│ Scenes │ │

│ History │ │

│ Settings │ │

│ │ │

└──────────────┴───────────────────────────────┘

Giải thích bằng lời:

Sidebar giúp user luôn biết vị trí hiện tại trong Project.

Khu vực chính thay đổi theo Story, Character hoặc Scene đang được chỉnh sửa.

Top area duy trì project context và các hành động cấp project.

# **7. Primary Navigation**

Primary navigation gồm:

### **Overview**

Tổng quan trạng thái project.

### **Story**

Quản lý story core và narrative information.

### **Characters**

Quản lý toàn bộ recurring characters.

### **Scenes**

Quản lý sequence và generation theo từng scene.

### **History**

Xem generation history.

### **Settings**

Cấu hình project.

# **8. Project Overview Screen**

## **Purpose**

Cho user cái nhìn nhanh về tiến độ của story.

## **Nội dung có thể gồm**

- Project title;

- Story title;

- Number of characters;

- Number of scenes;

- Completed scenes;

- Scenes without images;

- Running generations;

- Recent activity.

## **Primary Actions**

- Continue Story;

- Add Character;

- Add Scene;

- Generate next incomplete scene.

# **9. Story Workspace**

## **Purpose**

Quản lý thông tin tổng thể của câu chuyện.

## **Sections**

### **Story Summary**

- title;

- premise;

- synopsis;

- tone;

- genre.

### **World / Setting**

Thông tin bối cảnh quan trọng.

### **Story Facts**

Các dữ kiện cần giữ ổn định.

### **Relationships**

Quan hệ giữa characters.

### **Style Direction**

Visual direction của project.

# **10. Canonical vs AI Proposal UX**

Đây là một nguyên tắc quan trọng.

User phải phân biệt được:

Canonical Story Data

≠

AI Proposal

AI-generated proposal không được xuất hiện như thể nó đã trở thành dữ liệu chính thức.

Proposal nên có các hành động:

- Accept;

- Edit & Accept;

- Reject.

# **11. Character List**

Character List hiển thị toàn bộ recurring characters.

Mỗi character card có thể thể hiện:

- portrait/reference;

- name;

- role;

- short description;

- status;

- reference availability.

## **Primary Actions**

- Add Character;

- Open Character;

- Generate Character;

- Search / Filter nếu số lượng lớn.

# **12. Character Detail**

Character Detail nên được chia theo thông tin dễ hiểu thay vì technical memory structure.

Suggested sections:

Character

├── Profile

├── Identity

├── Appearance

├── References

└── Story Usage

# **13. Character Profile**

Bao gồm:

- name;

- role;

- personality;

- narrative description;

- relationships.

# **14. Character Identity**

Hiển thị những thuộc tính cần duy trì ổn định.

User có thể:

- edit;

- lock;

- unlock;

- review canonical values.

Locked attributes phải có indicator rõ ràng.

# **15. Character Appearance**

Tách khỏi Identity.

Bao gồm:

- default appearance;

- outfit;

- style-related information;

- mutable attributes.

UX phải giúp user hiểu rằng thay áo hoặc biểu cảm không đồng nghĩa tạo nhân vật mới.

# **16. Character References**

Hiển thị:

- canonical reference;

- alternative references;

- generated references.

User có thể:

- upload;

- generate;

- compare;

- set as canonical.

Canonical reference phải luôn dễ nhận biết.

# **17. Scene List**

Scene List là một trong các màn hình quan trọng nhất.

Mỗi Scene card nên thể hiện tối thiểu:

- scene order;

- scene title;

- short summary;

- active characters;

- image status;

- generation status.

Ví dụ:

Scene 08 — Train Station

\[A\] \[B\]

Image: Selected

Status: Complete

# **18. Scene Ordering**

User phải có thể reorder Scene trực quan.

Preferred interaction:

- drag-and-drop;

- hoặc explicit move controls.

Khi reorder:

- Scene ID không thay đổi;

- generated history không mất;

- chỉ sequence order thay đổi.

# **19. Scene Detail Workspace**

Scene Detail là trung tâm của workflow tạo nội dung.

Suggested layout:

┌─────────────────────────────────────────────┐

│ Scene Header │

├────────────────┬────────────────────────────┤

│ Scene Content │ Visual Generation Panel │

│ │ │

│ Summary │ Selected Image │

│ Setting │ │

│ Characters │ Candidates │

│ Action │ │

│ Emotion │ Generate / Regenerate │

└────────────────┴────────────────────────────┘

Giải thích bằng lời:

Bên trái là những gì scene **có nghĩa trong story**.

Bên phải là những gì AI **đã tạo ra cho scene**.

Cách chia này giúp user phân biệt story data và generated output.

# **20. Scene Character Assignment**

Trong Scene Detail, user phải dễ dàng thấy:

**Characters in this scene**

User có thể:

- add character;

- remove character;

- chỉnh scene-specific state.

# **21. Scene Character State**

Khi character được thêm vào Scene, user có thể đặt:

- clothing;

- expression;

- action;

- temporary condition;

- appearance note.

Các thuộc tính này chỉ áp dụng cho Scene hiện tại trừ khi user thực hiện explicit canonical update.

# **22. Generation Action**

Primary generation action tại Scene:

**Generate Image**

Không nên yêu cầu user nhập technical prompt ở default workflow.

System tự xây prompt từ Scene Context.

# **23. Advanced Generation Controls**

Advanced controls có thể được cung cấp nhưng nên hidden/collapsed mặc định.

Ví dụ:

- additional instruction;

- candidate count;

- composition hint;

- camera framing.

Technical model parameters chỉ nên xuất hiện nếu thật sự cần thiết.

# **24. Generation Progress**

Khi generation đang chạy, user phải nhìn thấy trạng thái.

Ví dụ:

Preparing scene...

Generating image...

Checking consistency...

Done

Không cần expose internal service names.

# **25. Non-Blocking Generation UX**

Generation không được khóa toàn bộ application.

User nên có thể:

- chuyển Scene;

- xem Character;

- chỉnh Story;

- review output khác.

Running job cần tiếp tục ở background job layer.

# **26. Candidate Review**

Generated candidates được hiển thị dưới dạng visual grid hoặc carousel.

Mỗi candidate có các action:

- Select;

- View;

- Regenerate from this;

- Archive/Discard nếu hỗ trợ.

# **27. Selected Candidate**

Selected candidate phải có trạng thái dễ nhận biết.

Ví dụ:

**Selected**

Không chỉ dựa vào border color; cần có icon hoặc label để tránh vấn đề accessibility.

# **28. Consistency Warning UX**

Nếu hệ thống phát hiện warning:

Ví dụ:

> Character appearance may differ from previous scenes.

Warning phải:

- dễ thấy;

- không gây hoảng;

- giải thích được;

- đưa ra hành động.

Possible actions:

- Review;

- Regenerate;

- Keep Anyway.

# **29. Blocking Validation**

Nếu candidate không thể được sử dụng:

UI phải phân biệt rõ với warning.

Ví dụ:

> This result could not be used because the generated image is invalid.

Action:

- Retry;

- Generate Again.

# **30. Regeneration UX**

Regenerate phải nằm gần selected output hoặc candidate.

Khi user chọn Regenerate:

### **Default**

Generate another version.

### **Optional Instruction**

User có thể nhập:

> Make the character look more worried.

Không cần chỉnh toàn bộ prompt kỹ thuật.

# **31. Generation History UX**

History nên được xem theo:

- Scene;

- timeline;

- generation attempt.

User có thể thấy:

Attempt 1

Candidate A

Attempt 2

Candidate B — Selected

Attempt 3

Candidate C

# **32. Outdated Context UX**

Nếu Story hoặc Character thay đổi sau generation:

Candidate có thể nhận label:

**Generated with older character data**

Thay vì dùng thuật ngữ technical như:

CONTEXT_VERSION_MISMATCH.

User actions:

- Keep;

- Regenerate with latest data.

# **33. Unsaved Changes**

Nếu user chỉnh Scene rồi chọn Generate khi chưa save:

UX phải tránh ambiguity.

Preferred behavior:

- auto-save validated changes;

- hoặc yêu cầu Save & Generate.

Không được generation từ dữ liệu không rõ version.

# **34. AI Proposal Pattern**

Các AI proposal dạng text nên sử dụng pattern thống nhất:

AI Suggestion

\[Generated Content\]

Accept

Edit

Reject

Không update canonical field trước khi workflow xác nhận.

# **35. Empty States**

Empty state phải hướng dẫn user hành động tiếp theo.

Ví dụ Character Empty State:

> No characters yet.  
> Add your first character or let AI suggest one from your story.

Actions:

- Add Character;

- Generate Suggestions.

# **36. Scene Empty State**

Ví dụ:

> Your story does not have any scenes yet.

Actions:

- Add Scene;

- Generate Scene Outline.

# **37. Loading States**

Không nên hiển thị spinner vô thời hạn mà không có context.

Generation operation nên có:

- activity state;

- task description;

- ability to leave screen.

# **38. Error States**

Error message phải ưu tiên ngôn ngữ nghiệp vụ.

Không nên:

> HTTP 500 - diffusion worker unavailable.

Nên:

> Image generation failed. Your scene data is safe. Try again.

Technical details có thể nằm trong expandable section nếu cần debugging.

# **39. Confirmation Strategy**

Không nên confirmation cho mọi hành động nhỏ.

Confirmation nên dùng cho:

- delete Project;

- delete Character có dependencies;

- delete Scene có generated assets;

- replace canonical reference nếu có ảnh hưởng đáng kể.

# **40. Destructive Action UX**

Destructive actions phải:

- rõ ràng;

- phân biệt với primary actions;

- giải thích impact.

Ví dụ:

> Deleting this character may affect 12 scenes.

# **41. Dependency Awareness**

Nếu Character đang được sử dụng trong nhiều Scene:

Character Detail nên có:

**Used in 12 scenes**

User có thể mở danh sách scene liên quan.

# **42. Mobile Scope**

MVP ưu tiên desktop/web workspace.

Responsive support nên đảm bảo:

- đọc project;

- review content;

- basic editing.

Advanced creative workspace không bắt buộc tối ưu hoàn toàn cho mobile trong MVP.

# **43. UX Flow — Create Story**

Home

↓

Create Project

↓

Story Setup

↓

Enter Idea

↓

AI Suggestion

↓

Review

↓

Accept / Edit

↓

Project Workspace

Giải thích bằng lời:

Người dùng bắt đầu từ Home, tạo project rồi nhập ý tưởng. AI có thể hỗ trợ phát triển story nhưng user review trước khi dữ liệu trở thành nội dung chính thức.

# **44. UX Flow — Create Character**

Characters

↓

Add Character

↓

Manual / AI-Assisted

↓

Character Profile

↓

Identity

↓

Reference

↓

Save

# **45. UX Flow — Generate Scene Image**

Scenes

↓

Open Scene

↓

Review Scene Data

↓

Assign Characters

↓

Generate Image

↓

Generation Progress

↓

Candidate Review

↓

Select / Regenerate

# **46. UX Flow — Regeneration**

Selected Image

↓

Regenerate

↓

Optional Instruction

↓

New Generation Attempt

↓

New Candidate

↓

Compare

↓

Keep Old / Select New

# **47. UX Flow — Character Change Impact**

Edit Character

↓

Save New Canonical Data

↓

System Finds Affected Scenes

↓

Show Outdated Indicators

↓

User Reviews

↓

Optional Regeneration

# **48. User Mental Model**

UX phải xây dựng mental model đơn giản:

Story

↓

Characters + Scenes

↓

AI Generates

↓

I Review

↓

I Decide

Không phải:

Prompt

↓

Model

↓

Seed

↓

Sampler

↓

Embedding

↓

Attention

Application phải che giấu complexity khi complexity đó không giúp user sáng tác tốt hơn.

# **49. UX Requirements**

## **UX-FR-01**

User SHALL luôn xác định được Project hiện tại.

## **UX-FR-02**

User SHALL có thể truy cập Story, Characters và Scenes từ primary navigation.

## **UX-FR-03**

User SHALL phân biệt được canonical data với AI-generated proposal.

## **UX-FR-04**

User SHALL phân biệt được selected output với unselected candidates.

## **UX-FR-05**

User SHALL nhìn thấy generation status của Scene.

## **UX-FR-06**

User SHALL có thể regenerate mà không rời Scene context.

## **UX-FR-07**

User SHALL được cảnh báo khi output sử dụng outdated context.

## **UX-FR-08**

User SHALL thấy được locked character attributes.

## **UX-FR-09**

User SHALL biết những characters nào xuất hiện trong Scene.

## **UX-FR-10**

AI failure SHALL được thể hiện mà không làm user hiểu rằng Story đã bị mất.

# **50. Usability Acceptance Criteria**

### **AC-UX01-01**

User có thể từ Home mở một Project hiện có trong tối đa một navigation action từ project list.

### **AC-UX01-02**

Từ Scene Detail, user có thể thực hiện Generate Image mà không cần mở màn hình technical configuration.

### **AC-UX01-03**

User có thể xác định candidate nào đang selected mà không cần mở Generation History.

### **AC-UX01-04**

User có thể xác định active characters của Scene trực tiếp trong Scene workspace.

### **AC-UX01-05**

Khi generation đang chạy, user vẫn có thể điều hướng sang khu vực khác của Project.

### **AC-UX01-06**

Nếu Character data thay đổi, user có thể nhận biết output nào có khả năng outdated.

### **AC-UX01-07**

Warning và blocking error phải được phân biệt rõ ràng.

# **51. Dependency on Product Documents**

UX-01 hiện thực hóa:

- PROD-01 product requirements;

- PROD-02 functional workflows.

UX-01 không được tự tạo business rule mới làm thay đổi Product Specification.

Nếu UX cần workflow khác với PROD-02, Product Specification phải được review và cập nhật trước.

# **52. Out of Scope**

UX-01 không quy định:

- exact font;

- color palette;

- spacing tokens;

- component pixel dimensions;

- animation duration;

- 3D visual language;

- illustration style;

- responsive breakpoint cụ thể.

Các nội dung này thuộc UX-02.

# **53. Core UX Principle**

Nguyên tắc trung tâm của UX:

> **Keep the creator inside the story, not inside the AI system.**

Người dùng cần tập trung vào:

- câu chuyện;

- nhân vật;

- scene;

- hình ảnh;

- quyết định sáng tạo.

AI infrastructure phải hoạt động phía sau và chỉ xuất hiện khi user cần tương tác với kết quả hoặc xử lý vấn đề.
