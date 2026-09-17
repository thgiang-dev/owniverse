# **UX-02 — UI Design System & Interaction Specification**

**Document ID:** UX-02  
**Document Type:** UI Design System & Interaction Specification  
**Product:** AI-Assisted Story Creation Studio  
**Project:** Character-Consistent Long-Range Story Generation  
**Primary Role:** Product Designer / UI Designer  
**Status:** Draft v1  
**Parent Documents:** PROD-01, PROD-02, UX-01  
**Related Documents:** ARCH-01, AI-01, AI-02, AI-03

# **1. Purpose**

Tài liệu này định nghĩa ngôn ngữ thiết kế giao diện và các quy tắc tương tác cho AI-Assisted Story Creation Studio.

Mục tiêu là đảm bảo toàn bộ sản phẩm có:

- visual identity nhất quán;

- hierarchy rõ ràng;

- interaction dễ hiểu;

- trạng thái AI dễ theo dõi;

- trải nghiệm sáng tạo hiện đại;

- animation có mục đích;

- sử dụng 3D và visual effects có kiểm soát;

- khả năng mở rộng khi thêm màn hình và tính năng mới.

Tài liệu này không xác định business workflow hoặc backend behavior.

# **2. Design Vision**

Sản phẩm nên tạo cảm giác như một **creative studio hiện đại dành cho story creator**, kết hợp giữa:

- editorial workspace;

- creative software;

- AI assistant;

- cinematic storytelling environment.

Giao diện không nên giống:

- admin dashboard truyền thống;

- AI chatbot thuần túy;

- form management system;

- model playground dành cho kỹ sư.

Người dùng phải cảm thấy họ đang bước vào một không gian để **xây dựng thế giới, nhân vật và câu chuyện**.

# **3. Core Visual Principles**

## **UI-PR-01 — Story First**

Visual hierarchy phải ưu tiên:

1.  story content;

2.  character;

3.  scene;

4.  generated artwork;

5.  AI controls.

AI không được chiếm vai trò hình ảnh lớn hơn nội dung sáng tạo.

## **UI-PR-02 — Expressive but Controlled**

Giao diện được phép sử dụng:

- gradient;

- glow;

- depth;

- glass-like surfaces;

- soft shadows;

- animated transitions;

- subtle 3D objects.

Nhưng không được làm giảm:

- readability;

- navigation clarity;

- performance;

- accessibility.

## **UI-PR-03 — Content Is the Hero**

Generated artwork và character visuals là yếu tố thị giác mạnh nhất.

UI chrome phải hỗ trợ nội dung chứ không cạnh tranh với nội dung.

## **UI-PR-04 — Motion Communicates Meaning**

Animation phải thể hiện:

- state change;

- spatial relationship;

- progression;

- system feedback.

Animation không được tồn tại chỉ để trang trí mọi interaction.

## **UI-PR-05 — Progressive Complexity**

UI mặc định phải đơn giản.

Các control nâng cao chỉ xuất hiện khi:

- user mở Advanced Settings;

- context thực sự cần;

- user có intent chỉnh sâu.

# **4. Visual Direction**

Visual direction đề xuất:

**Bright Futuristic Creative Studio**

Đặc trưng:

- nền sáng hoặc trung tính;

- accent color có saturation cao;

- gradient tươi sáng;

- card mềm;

- border tinh tế;

- shadow tạo chiều sâu;

- hình minh họa / artwork nổi bật;

- motion mềm;

- 3D decorative element có giới hạn.

Phong cách cần cảm giác:

- creative;

- energetic;

- premium;

- friendly;

- modern.

Không nên quá:

- corporate;

- cyberpunk tối;

- gaming-heavy;

- childish.

# **5. Theme Strategy**

Hệ thống SHOULD hỗ trợ:

- Light Theme;

- Dark Theme.

## **Light Theme**

Là theme mặc định cho creative workspace.

Ưu tiên:

- canvas sáng;

- neutral background;

- vivid accent;

- card elevation nhẹ.

## **Dark Theme**

Tối ưu cho:

- image review;

- long creative sessions;

- cinematic artwork viewing.

Cả hai theme phải dùng cùng semantic token system.

# **6. Color System**

Color system phải dựa trên **semantic roles**, không hard-code màu theo component.

Các nhóm token chính:

color.background

color.surface

color.surfaceElevated

color.textPrimary

color.textSecondary

color.textMuted

color.border

color.primary

color.secondary

color.accent

color.success

color.warning

color.error

color.info

# **7. Accent Strategy**

Primary accent nên có cảm giác:

- sáng;

- sáng tạo;

- công nghệ;

- có thể hoạt động tốt trên cả light và dark theme.

Secondary accent có thể dùng để tạo:

- gradient;

- active state;

- artwork framing;

- hero section.

Không nên sử dụng quá nhiều màu accent trong cùng một screen.

# **8. Gradient Usage**

Gradient được phép dùng tại:

- hero section;

- primary CTA;

- selected creative state;

- AI activity indicator;

- empty-state illustration;

- decorative background.

Không dùng gradient cho:

- paragraph text;

- dense data;

- form fields;

- long table rows.

# **9. Typography**

Typography phải ưu tiên readability nhưng vẫn có tính sáng tạo.

## **Display Typography**

Dùng cho:

- landing hero;

- major project title;

- creative section title.

Có thể sử dụng font có personality cao hơn.

## **Interface Typography**

Dùng cho:

- navigation;

- field label;

- body;

- form;

- status;

- button.

Phải ưu tiên readability.

# **10. Typography Scale**

Suggested semantic hierarchy:

Display

Heading 1

Heading 2

Heading 3

Title

Body

Body Small

Label

Caption

Không sử dụng quá nhiều font size tùy ý.

# **11. Layout Grid**

Desktop workspace nên sử dụng responsive grid.

Suggested structure:

12-column content grid

Spacing dựa trên một base scale nhất quán.

Ví dụ:

4

8

12

16

24

32

48

64

Exact pixel values có thể điều chỉnh khi implementation.

# **12. Page Structure**

Typical application page:

Top Bar

↓

Primary Navigation / Sidebar

↓

Page Header

↓

Context Actions

↓

Main Content

↓

Secondary Panels / Inspector

# **13. Sidebar**

Sidebar là navigation chính trong Project Workspace.

Bao gồm:

- Overview;

- Story;

- Characters;

- Scenes;

- History;

- Settings.

## **States**

- default;

- hover;

- active;

- collapsed;

- notification indicator.

Active item phải được xác định bằng nhiều tín hiệu, không chỉ màu.

# **14. Top Bar**

Top Bar có thể chứa:

- Project name;

- breadcrumb;

- global status;

- save state;

- theme control;

- account actions.

Không nên chứa quá nhiều primary action.

# **15. Page Header**

Page Header gồm:

- page title;

- optional description;

- status;

- primary action;

- contextual secondary actions.

Ví dụ Scene page:

Scene 08 — Train Station

Draft · 2 Characters · Image Generated

\[Generate Image\]

# **16. Card System**

Card là component quan trọng vì được dùng cho:

- projects;

- characters;

- scenes;

- candidates;

- assets.

Card nên có:

- clear hierarchy;

- medium corner radius;

- subtle border;

- minimal shadow;

- hover feedback.

# **17. Project Card**

Project Card hiển thị:

- cover/preview;

- project title;

- progress;

- last edited;

- quick action.

Hover có thể tạo:

- slight elevation;

- image movement;

- soft glow.

Không nên sử dụng dramatic 3D tilt trong dense project list.

# **18. Character Card**

Character card ưu tiên portrait.

Content:

- portrait;

- name;

- role;

- scene count;

- reference state.

Possible status:

- Ready;

- Missing Reference;

- Needs Review.

# **19. Scene Card**

Scene card phải cho user đọc trạng thái nhanh.

Suggested structure:

Scene number

Thumbnail

Scene title

Characters

Generation status

Selected output status

# **20. Candidate Card**

Candidate Card là visual-first component.

Phải hỗ trợ:

- image preview;

- validation state;

- selected badge;

- warning badge;

- actions.

Selected candidate phải có strong visual indicator.

# **21. Form Design**

Form controls phải dùng consistent pattern.

Common structure:

Label

Input

Helper / Validation message

Fields không nên dựa hoàn toàn vào placeholder để mô tả ý nghĩa.

# **22. AI-Assisted Input Pattern**

Với field có AI assistance:

Story Synopsis

\[Text Editor\]

\[Ask AI\]

AI action nên là secondary action.

User phải có thể sử dụng field mà không cần AI.

# **23. AI Proposal Component**

AI Proposal phải có visual treatment riêng.

Suggested component:

┌─────────────────────────────┐

│ ✦ AI Suggestion │

│ │

│ Proposed content... │

│ │

│ \[Accept\] \[Edit\] \[Reject\] │

└─────────────────────────────┘

Proposal không được visual giống canonical data.

# **24. Canonical Data Indicator**

Canonical state có thể sử dụng:

- Saved;

- Current;

- lock indicator;

- subtle canonical marker.

Không cần hiển thị từ technical "canonical" cho end user nếu gây khó hiểu.

User-facing wording có thể là:

- Current Story;

- Saved Character Details;

- Current Reference.

# **25. Locked Attribute Interaction**

Locked field hiển thị lock icon.

Interaction:

Unlocked

↓ click

Locked

Khi locked:

- AI cannot overwrite;

- user vẫn có thể manually edit sau explicit action.

Tooltip có thể giải thích:

> AI will keep this detail unchanged.

# **26. Button Hierarchy**

Buttons được chia thành:

### **Primary**

Hành động chính của context.

Ví dụ:

- Generate;

- Save;

- Select.

### **Secondary**

Hành động thay thế.

Ví dụ:

- Edit;

- Preview;

- Compare.

### **Tertiary**

Low emphasis.

### **Destructive**

Delete hoặc destructive actions.

Một khu vực không nên có nhiều hơn một action mang visual weight của Primary nếu không thật sự cần.

# **27. Iconography**

Icon phải:

- đơn giản;

- dễ hiểu;

- cùng visual family.

Không dùng icon thay text nếu meaning không phổ biến.

Ví dụ nên có label cho:

- Regenerate;

- Set as Reference;

- Archive.

# **28. Status System**

Status phải dùng combination:

Color + Icon + Label

Ví dụ:

- [x] Complete;

- ◌ Generating;

- ! Warning;

- × Failed.

Không dùng màu đơn độc.

# **29. Generation Status Component**

Generation progress có thể hiển thị:

Preparing

Generating

Checking

Complete

Có thể sử dụng animated progress indicator.

Không cần expose technical pipeline stages nếu không mang giá trị cho user.

# **30. AI Activity Animation**

AI processing có thể dùng:

- moving gradient;

- subtle shimmer;

- animated particles;

- soft pulse.

Không dùng animation quá mạnh hoặc nhấp nháy liên tục.

# **31. Motion Principles**

Motion dùng để:

### **Orientation**

Giúp user hiểu component đến từ đâu.

### **Feedback**

Cho biết interaction thành công.

### **Continuity**

Giữ context giữa screen changes.

### **Delight**

Tạo cảm giác sáng tạo ở một số high-value moments.

# **32. Motion Duration**

Interaction nhỏ nên nhanh.

Ví dụ:

- hover;

- button feedback;

- tooltip.

Page/Panel transition có thể chậm hơn một chút.

Không dùng animation dài làm user phải chờ.

# **33. Page Transition**

Page navigation trong Project Workspace nên sử dụng transition nhẹ.

Ví dụ:

- content fade;

- slight slide;

- shared element movement.

Sidebar giữ ổn định để user không mất orientation.

# **34. Scene Transition**

Khi chuyển Scene:

- Scene list giữ nguyên vị trí;

- Scene content transition nhẹ;

- image panel cập nhật.

Không reload toàn bộ workspace nếu không cần thiết.

# **35. Candidate Generation Animation**

Khi candidate mới xuất hiện:

1.  placeholder được tạo;

2.  generation state hiển thị;

3.  image fade in sau khi ready;

4.  validation badge xuất hiện sau validation.

Animation phải phản ánh lifecycle thực tế.

# **36. 3D Design Strategy**

3D được sử dụng như **supporting visual language**, không phải UI navigation chính.

Recommended uses:

- landing hero;

- empty state;

- project cover;

- character visual showcase;

- transition decorative element.

# **37. 3D Usage Restrictions**

Không nên sử dụng complex 3D trong:

- forms;

- dense lists;

- editor panels;

- generation history;

- settings.

Không được yêu cầu GPU-heavy rendering chỉ để hiển thị basic interface.

# **38. Hero Experience**

Landing/Home có thể dùng một 3D visual đại diện cho quá trình:

Idea

↓

Character

↓

Scene

↓

Story World

Ví dụ:

một cuốn truyện hoặc stack panel trong không gian 3D nhẹ, artwork nổi lên từ các trang khi user scroll hoặc move pointer.

Mục đích là truyền đạt sản phẩm, không chỉ trang trí.

# **39. Parallax**

Parallax có thể sử dụng ở:

- landing;

- onboarding;

- project empty state.

Không nên sử dụng trong editor thường xuyên vì có thể gây distraction.

# **40. Hover Interactions**

Card hover có thể:

- nâng nhẹ;

- tăng border contrast;

- reveal secondary action;

- animate thumbnail rất nhẹ.

Không nên:

- rotate lớn;

- zoom quá mạnh;

- gây layout shift.

# **41. Microinteractions**

Các microinteraction phù hợp:

- lock icon closes;

- save checkmark;

- candidate becomes selected;

- scene dragged to new position;

- generation starts;

- output warning appears.

# **42. Selected Candidate Interaction**

Khi chọn candidate:

1.  selected state chuyển sang candidate mới;

2.  previous selected state mất highlight;

3.  animation ngắn xác nhận;

4.  preview chính cập nhật.

History không thay đổi.

# **43. Regeneration Interaction**

Khi user nhấn Regenerate:

UI có thể mở compact panel:

Generate another version

Additional instruction

\[\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\]

\[Cancel\] \[Generate\]

Advanced parameters nằm trong expandable control.

# **44. Scene Workspace Layout**

Desktop scene workspace nên ưu tiên split layout:

Story Context / Scene Editor

\+

Generated Visual

Recommended ratio có thể dao động:

40 / 60

hoặc:

50 / 50

tùy mode.

Image review mode có thể mở rộng visual panel.

# **45. Focus Mode**

User SHOULD có thể mở Focus Mode cho artwork.

Focus Mode:

- ẩn phần navigation không cần thiết;

- dùng background tối hơn;

- hiển thị image lớn;

- hỗ trợ next/previous candidate;

- show essential actions only.

# **46. Character Workspace Layout**

Suggested:

Character Portrait

\+

Profile / Identity / Appearance

\+

References

\+

Story Usage

Portrait giữ persistent visibility trên desktop nếu không ảnh hưởng viewport.

# **47. Story Workspace Layout**

Long-form text nên có max-width để đọc dễ.

Không kéo paragraph toàn chiều rộng màn hình lớn.

Story facts có thể dùng structured card / sections thay vì một textarea khổng lồ.

# **48. Scene Timeline**

Scene sequence có thể hỗ trợ:

- vertical list;

- horizontal storyboard;

- compact timeline.

MVP có thể bắt đầu bằng vertical/list layout.

Storyboard view là enhancement phù hợp sau đó.

# **49. Thumbnail Strategy**

Scene thumbnail ưu tiên:

1.  selected image;

2.  latest valid candidate;

3.  placeholder nếu chưa generation.

Không dùng failed candidate làm default thumbnail.

# **50. Empty State Visual Language**

Empty state nên có:

- illustration / subtle 3D visual;

- short explanation;

- one primary action;

- optional secondary AI action.

Không hiển thị trang trắng hoàn toàn.

# **51. Character Empty State**

Ví dụ:

Bring your first character to life.

Create one manually or let AI help you

build a character from your story.

\[Create Character\]

Generate with AI

# **52. Scene Empty State**

Ví dụ:

Turn your story into scenes.

Create your first scene or generate

an initial scene outline.

\[Create Scene\]

Generate Outline

# **53. Error Visual Language**

Error phải rõ nhưng không phá creative flow.

Component gồm:

- error title;

- concise explanation;

- recovery action.

Ví dụ:

Image generation failed

Your scene and previous images are safe.

\[Try Again\]

# **54. Warning Visual Language**

Warnings dùng khi user vẫn có thể tiếp tục.

Ví dụ:

Character may look different

This image appears less consistent with

the selected character reference.

\[Review\] \[Regenerate\] \[Keep\]

# **55. Stale Output State**

Outdated candidate cần visual indicator nhẹ.

Ví dụ badge:

**Older Character Data**

Tooltip:

> This image was generated before the character was updated.

Actions:

- Regenerate;

- Keep.

# **56. Toast Notifications**

Toast phù hợp cho:

- saved;

- copied;

- archived;

- queued;

- non-critical completion.

Không dùng toast cho lỗi cần user xử lý lâu dài.

# **57. Modal Usage**

Modal chỉ dùng cho:

- high-impact confirmation;

- compact creation;

- decision requiring temporary focus.

Không dùng modal cho long-form editing.

# **58. Drawer / Side Panel**

Drawer phù hợp cho:

- advanced generation settings;

- candidate details;

- metadata;

- activity;

- quick edit.

User vẫn nhìn thấy context chính phía sau.

# **59. Tooltip Usage**

Tooltip dùng cho:

- unfamiliar icons;

- lock behavior;

- validation indicator;

- technical optional settings.

Không đặt critical information chỉ trong tooltip.

# **60. Accessibility**

UI phải đáp ứng các nguyên tắc cơ bản:

- đủ color contrast;

- keyboard navigable;

- visible focus state;

- labels cho icon button;

- semantic HTML khi triển khai web;

- animation có reduced-motion alternative;

- status không dựa chỉ vào màu.

# **61. Reduced Motion**

Nếu user/device bật reduced motion:

System phải giảm hoặc tắt:

- parallax;

- decorative 3D movement;

- large transitions;

- looping visual effects.

Functional state feedback vẫn phải tồn tại.

# **62. Responsive Strategy**

## **Desktop**

Primary creative environment.

Full feature layout.

## **Tablet**

Supported với adjusted panels.

Có thể chuyển side panels thành drawer.

## **Mobile**

Ưu tiên:

- review;

- light editing;

- status monitoring.

Complex creative editing không bắt buộc đạt parity trong MVP.

# **63. Breakpoint Behaviour**

Khi viewport giảm:

Sidebar

↓

Collapsible Sidebar

↓

Navigation Drawer

Split editor:

Two Columns

↓

Resizable

↓

Tabbed / Stacked

# **64. Loading Skeleton**

Content loading nên dùng skeleton khi layout đã biết.

Ví dụ:

- project cards;

- character list;

- scene cards.

Generation image dùng placeholder riêng thay vì generic skeleton.

# **65. Save State**

Nếu có auto-save:

Top bar có thể hiển thị:

- Saving…;

- Saved;

- Save Failed.

Không spam user bằng notification mỗi lần auto-save thành công.

# **66. Drag and Drop**

Drag-and-drop có thể dùng cho:

- reorder scenes;

- upload references;

- reorganize candidate/assets nếu cần.

Luôn cung cấp keyboard hoặc explicit control thay thế cho action quan trọng.

# **67. Scene Reordering Motion**

Khi drag Scene:

- dragged card nâng lên;

- placeholder thể hiện vị trí mới;

- surrounding cards dịch chuyển mềm;

- order cập nhật sau drop.

# **68. AI vs Manual Controls**

AI action nên dùng visual signature riêng, ví dụ:

- sparkle icon;

- subtle gradient;

- AI label khi cần.

Không phải mọi button trong app đều mang AI styling.

# **69. Advanced Settings**

Generation settings chia thành:

### **Basic**

- instruction;

- candidate count;

- composition preference.

### **Advanced**

Model-specific settings chỉ được expose nếu product thật sự cần.

Default user không cần nhìn:

- sampler;

- scheduler;

- inference internals;

- embeddings.

# **70. Design Token Categories**

Design system cần ít nhất:

Color

Typography

Spacing

Radius

Border

Shadow

Motion

Z-index

Breakpoint

# **71. Component Inventory**

Core component library gồm:

- Button;

- IconButton;

- Input;

- Textarea;

- Select;

- Checkbox;

- Switch;

- Tabs;

- Badge;

- Tooltip;

- Toast;

- Modal;

- Drawer;

- Dropdown;

- Breadcrumb;

- SidebarItem;

- ProjectCard;

- CharacterCard;

- SceneCard;

- CandidateCard;

- AIProposal;

- GenerationStatus;

- ValidationWarning;

- EmptyState;

- AssetPreview.

# **72. Component State Requirements**

Interactive component phải xác định:

- default;

- hover;

- active;

- focus;

- disabled;

- loading;

- error nếu phù hợp.

# **73. Primary CTA Rules**

Primary CTA phải phản ánh task hiện tại.

Ví dụ:

Home:

**Create Project**

Scene:

**Generate Image**

Character:

**Save Character** hoặc context-equivalent.

Không dùng một global AI button khổng lồ cho mọi operation.

# **74. Confirmation Copy**

Copy nên nói rõ hậu quả.

Không:

> Are you sure?

Nên:

> Delete Character A?
>
> This character is used in 8 scenes. Existing generated images will remain, but those scenes will no longer reference this character.

# **75. Tone of UI Copy**

UI copy phải:

- ngắn;

- rõ;

- thân thiện;

- không quá kỹ thuật.

Không dùng wording quảng cáo trong functional workspace.

# **76. AI Transparency**

User cần được biết khi content:

- AI-generated;

- AI-suggested;

- currently generating;

- validated;

- potentially inconsistent.

Không cần expose internal chain-of-thought hoặc model reasoning.

# **77. Character Consistency UI**

Consistency nên xuất hiện như quality assistance.

Possible indicator:

Consistency

Good

hoặc:

Needs Review

Không cần hiển thị raw metric nếu metric không có ý nghĩa với user.

# **78. Validation Detail**

Nếu user mở detail:

Character consistency

Potential hairstyle difference

Scene alignment

Passed

Image quality

Passed

UI presentation có thể đơn giản hơn technical validator output.

# **79. Landing Page Direction**

Landing page có thể gồm:

1.  Hero;

2.  Product visual;

3.  How it works;

4.  Character consistency showcase;

5.  Multi-scene workflow;

6.  CTA.

Không cần đưa toàn bộ editor features lên landing.

# **80. Hero Interaction**

Hero có thể dùng interactive scene stack.

Ví dụ:

User scroll:

Story Idea

↓

Character portrait appears

↓

Multiple scene cards spread out

↓

Same character appears across scenes

Interaction này trực tiếp truyền tải value proposition về character consistency.

# **81. Visual Showcase**

Một showcase hiệu quả nên hiển thị:

Scene 01 Scene 08 Scene 15

Character A Character A Character A

với bối cảnh, pose và clothing có thể thay đổi nhưng identity vẫn nhận biết được.

# **82. Animation Performance**

Decorative effects không được:

- chặn first interaction;

- làm editor lag;

- chạy nặng trên mọi component;

- ảnh hưởng image scrolling.

Heavy visual effects phải có graceful fallback.

# **83. Asset Quality**

Generated images cần được hiển thị đúng aspect ratio.

Không stretch artwork.

Dùng:

- contain;

- cover;

tùy component purpose.

# **84. Image Inspection**

Full-screen preview nên hỗ trợ:

- zoom;

- fit;

- next/previous;

- validation info;

- select;

- regenerate.

# **85. Design Consistency**

Không tạo visual system riêng cho từng page.

Story, Character và Scene phải chia sẻ:

- typography;

- controls;

- spacing;

- card language;

- status language.

# **86. UX/UI Requirements**

## **UI-FR-01**

System SHALL có consistent design tokens.

## **UI-FR-02**

Primary navigation SHALL giữ visual consistency giữa Project pages.

## **UI-FR-03**

AI-generated proposal SHALL có visual treatment khác canonical content.

## **UI-FR-04**

Selected candidate SHALL có explicit status indicator.

## **UI-FR-05**

Generation lifecycle SHALL có visual feedback.

## **UI-FR-06**

Warnings SHALL có actionable next steps.

## **UI-FR-07**

Locked attributes SHALL có visible indicator.

## **UI-FR-08**

Long-running generation SHALL không block navigation.

## **UI-FR-09**

UI SHALL support reduced-motion behavior.

## **UI-FR-10**

Critical state SHALL không được truyền đạt chỉ bằng color.

# **87. Acceptance Criteria**

### **AC-UI02-01**

User có thể phân biệt primary action và secondary action của một screen mà không cần hướng dẫn.

### **AC-UI02-02**

User có thể phân biệt AI proposal với saved story data.

### **AC-UI02-03**

User có thể nhận ra selected candidate bằng label hoặc icon ngoài color.

### **AC-UI02-04**

Generation đang chạy phải có visible progress state.

### **AC-UI02-05**

Animation không được ngăn user tiếp tục navigation.

### **AC-UI02-06**

Reduced-motion mode không làm mất functional feedback.

### **AC-UI02-07**

Character warning phải cung cấp ít nhất một recovery action.

### **AC-UI02-08**

Mobile viewport không được làm mất khả năng xem và review story content chính.

# **88. Out of Scope**

UX-02 không định nghĩa:

- database schema;

- frontend framework;

- CSS implementation;

- WebGL engine;

- exact animation library;

- backend status protocol;

- AI model;

- model inference UI debugging.

Implementation được quyết định trong Architecture và Frontend technical design.

# **89. Relationship Between UX-01 and UX-02**

UX-01

Information Architecture & UX Flow

↓

Where does the user go?

What does the user do?

UX-02

UI Design System & Interaction

↓

What does it look like?

How does it respond?

Nói đơn giản:

**UX-01 thiết kế hành trình.**

**UX-02 thiết kế cách hành trình đó được thể hiện và tương tác trên giao diện.**

# **90. Final Design Principle**

Nguyên tắc cuối cùng:

> **Make AI feel powerful in the background, while creativity remains visually in the foreground.**

Giao diện có thể hiện đại, nhiều chiều sâu, animation và 3D, nhưng người dùng phải luôn tập trung vào:

- story;

- character;

- scene;

- artwork;

- creative decisions.

Visual effects chỉ được giữ lại khi chúng làm trải nghiệm sáng tạo tốt hơn.
