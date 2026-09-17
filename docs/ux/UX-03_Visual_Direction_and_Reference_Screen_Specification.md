# **UX-03 — Visual Direction & Reference Screen Specification**

**Document ID:** UX-03  
**Document Type:** Visual Direction & Reference Screen Specification  
**Product:** Owniverse — AI-Assisted Story Creation Studio  
**Project:** Character-Consistent Long-Range Story Generation  
**Primary Roles:** Art Director / Product Designer / UI Designer  
**Status:** Draft v1  
**Parent Documents:** PROD-01, PROD-02, UX-01, UX-02  
**Primary Visual References:** VR-01, VR-02

# **1. Purpose**

Tài liệu này là **visual source of truth** của Owniverse.

UX-01 xác định người dùng đi đâu và thực hiện workflow nào. UX-02 xác định component, interaction và design-system behavior. UX-03 xác định **sản phẩm thực tế phải trông và tạo cảm giác như thế nào**.

Tài liệu này đặc tả:

- visual identity;

- color direction;

- typography direction;

- surface, border, shadow và lighting;

- artwork treatment;

- gradient và glow;

- light/dark theme;

- motion personality;

- 3D usage;

- visual hierarchy;

- reference screens;

- quy tắc để designer, frontend developer và AI có thể tái tạo visual direction mà không cần phụ thuộc hoàn toàn vào hình ảnh tham khảo.

# **2. Core Rule for AI Readability**

Một quyết định thiết kế quan trọng **không được tồn tại duy nhất trong hình ảnh**.

Mọi reference image phải đi kèm textual specification đủ chi tiết để một AI hoặc developer không nhìn thấy ảnh vẫn có thể hiểu:

- bố cục;

- tỷ lệ;

- hierarchy;

- màu sắc;

- typography;

- lighting;

- component style;

- image treatment;

- animation;

- interaction intent.

Hình ảnh là **visual reference**.

Text là **implementation specification**.

# **3. Visual Source-of-Truth Hierarchy**

Khi có mâu thuẫn giữa các nguồn tham khảo, ưu tiên theo thứ tự:

1.  Requirement được ghi rõ trong UX-03.

2.  Hai Primary Visual References VR-01 và VR-02.

3.  UX-02 Design System rules.

4.  Secondary visual references.

5.  Quyết định tạm thời của implementation.

Developer không được tự thay đổi visual direction dựa trên một reference ngoài hệ thống nếu điều đó xung đột với UX-03.

# **4. Visual Reference Index**

## **VR-01 — Owniverse Light Visual Direction**

**Role:** Primary light-theme reference.

VR-01 định nghĩa cảm giác tổng thể của Light Theme và marketing-facing surfaces.

Visual direction chính:

- nền trắng hoặc trắng lạnh có sắc lavender/blue rất nhẹ;

- lượng whitespace lớn;

- violet là màu thương hiệu chính;

- electric blue và pink/lilac được dùng làm accent;

- artwork anime có saturation cao hơn UI;

- artwork hòa trực tiếp vào hero thay vì bị nhốt trong card;

- UI chrome cố tình giảm contrast để artwork trở thành visual hero;

- card nhẹ, border mỏng, shadow rất mềm;

- typography kết hợp một display serif giàu tính editorial với UI sans-serif hiện đại;

- layout có cảm giác editorial hơn dashboard;

- visual depth được tạo bằng overlap, fade, glow và layer thay vì shadow nặng.

### **Visual intent**

Light Theme phải mang cảm giác:

**optimistic, imaginative, elegant, creative, open, premium.**

Không được trở thành:

- trắng vô cảm kiểu enterprise software;

- pastel quá trẻ con;

- neon quá mạnh;

- glassmorphism dày đặc.

# **5. VR-02 — Owniverse Dark Visual Direction**

**Role:** Primary dark-theme reference.

VR-02 giữ nguyên visual DNA của VR-01 nhưng chuyển sang environment tối thích hợp cho:

- long creative sessions;

- artwork review;

- cinematic content;

- focus mode;

- generation workflows.

Visual direction chính:

- navy gần đen thay cho pure black;

- surface có sắc blue-violet;

- violet glow rõ hơn Light Theme nhưng vẫn kiểm soát;

- artwork trở thành nguồn sáng thị giác;

- border low-contrast;

- selected state được hỗ trợ bằng violet/blue illumination;

- text chính gần trắng nhưng không dùng pure white ở mọi cấp;

- card hòa vào background thay vì nổi thành những box nặng.

Dark Theme không phải là Light Theme được đảo màu đơn giản.

Nó phải có lighting behavior riêng.

# **6. Brand Personality**

Owniverse được mô tả bằng sáu thuộc tính:

**Imaginative  
**Ứng dụng tạo cảm giác có thể biến một ý tưởng nhỏ thành cả thế giới.

**Creative  
**Giao diện khuyến khích khám phá và sáng tác.

**Cinematic  
**Artwork và scene phải có cảm giác giống storyboard/cinematic frame hơn là thumbnail dữ liệu.

**Intelligent  
**AI hiện diện nhưng không phô diễn technical complexity.

**Elegant  
**UI nhiều visual richness nhưng vẫn có restraint.

**Approachable  
**Người dùng không cần là designer hay AI engineer để sử dụng.

# **7. Visual Design Concept**

Visual concept chính thức:

> **Modern Anime Creative Studio with Soft Futuristic Depth**

Đây là sự kết hợp giữa:

- anime/manga visual culture;

- editorial composition;

- creative software workspace;

- futuristic lighting;

- restrained glass/depth effects.

Không sử dụng phong cách cyberpunk nặng.

Không sử dụng visual language giống game HUD.

Không sử dụng corporate SaaS dashboard làm visual baseline.

# **8. Visual Hierarchy**

Thứ tự visual dominance mặc định:

1.  Generated artwork / character artwork.

2.  Page hoặc Scene title.

3.  Primary creative action.

4.  Canonical creative content.

5.  Navigation.

6.  Metadata.

7.  Technical/AI state.

AI infrastructure phải nằm ở cuối hierarchy trừ khi user đang xử lý một AI-specific failure.

# **9. Brand Color Direction**

Primary brand family:

**Violet → Indigo → Electric Blue**

Supporting accent:

**Lilac / Pink**

Optional highlight:

**Soft Cyan**

Màu sắc phải gợi:

- imagination;

- AI creativity;

- dreamlike worlds;

- digital creation.

# **10. Recommended Brand Palette**

Các giá trị sau là baseline implementation. Designer có thể tinh chỉnh nhỏ sau visual QA nhưng không được thay đổi hue direction.

| **Token**     | **Light** | **Dark** | **Purpose**          |
|---------------|-----------|----------|----------------------|
| Brand Primary | \#6C4CFF  | \#8A70FF | CTA, active, brand   |
| Brand Deep    | \#5135E8  | \#6C54F4 | hover/emphasis       |
| Electric Blue | \#4B8CFF  | \#69A7FF | secondary glow       |
| Accent Lilac  | \#B86CFF  | \#C98BFF | creative accent      |
| Accent Pink   | \#E36DCE  | \#F08ADB | sparing highlight    |
| Accent Cyan   | \#64C7FF  | \#7BD4FF | information/lighting |

Các màu này là brand colors, không thay thế semantic colors như success/error.

# **11. Light Theme Neutral Palette**

### **Page Background**

\#F7F8FE – trắng lạnh rất nhẹ.

Các vùng hero có thể chuyển gần \#FFFFFF.

### **Surface**

\#FFFFFF

### **Soft Surface**

\#F2F4FC

### **Elevated Surface**

Trắng với subtle shadow hoặc translucency.

### **Primary Text**

\#171B33

Không sử dụng pure black cho body content.

### **Secondary Text**

\#626C88

### **Muted Text**

\#8A93AA

### **Border**

\#E3E7F2

# **12. Dark Theme Neutral Palette**

### **Page Background**

\#081224

### **Workspace Background**

\#0B162B

### **Surface**

\#101C34

### **Elevated Surface**

\#14213D

### **Primary Text**

\#F3F5FF

### **Secondary Text**

\#AAB4CE

### **Muted Text**

\#7F8AA6

### **Border**

Navy-blue border có opacity thấp, tương đương khoảng:

rgba(155, 174, 220, 0.14)

Không sử dụng pure black \#000000 làm canvas chính.

# **13. Gradient System**

Signature brand gradient:

**Violet → Purple → Electric Blue**

Example direction:

\#864CFF → \#6C4CFF → \#4B8CFF

Optional creative variation:

**Pink → Violet → Blue**

Dùng cho:

- primary marketing CTA;

- AI activity;

- selected creative moments;

- hero lighting;

- decorative atmospheric elements.

Không dùng cho:

- body text;

- toàn bộ card;

- dense table;

- standard input.

# **14. Glow**

Glow phải xuất hiện như **environment lighting**, không phải neon outline.

Dark Theme có thể dùng glow rõ hơn Light Theme.

Các vị trí phù hợp:

- primary CTA;

- active creative action;

- selected generation candidate;

- hero artwork;

- AI processing state.

Glow phải mềm và có bán kính lớn.

Không dùng sharp neon stroke.

# **15. Typography Direction**

Owniverse sử dụng hai typography personality.

## **Display Typeface**

Dùng cho:

- landing hero;

- marketing statement;

- large storytelling headline.

Direction:

- high-contrast serif;

- editorial;

- elegant;

- slightly literary.

Recommended implementation family:

**DM Serif Display**, **Playfair Display**, hoặc equivalent.

Không dùng display serif cho dense application controls.

# **16. Interface Typeface**

Dùng cho:

- navigation;

- form;

- paragraph;

- card;

- button;

- metadata;

- workspace.

Recommended family:

**Inter**, **Manrope**, hoặc equivalent modern sans-serif.

Typography phải clean và neutral để cân bằng với expressive artwork.

# **17. Hero Typography**

Hero headline có thể rất lớn, thường 56–80 px trên desktop marketing layout.

Headline phải có:

- line-height chặt;

- 3–4 lines tối đa;

- một keyword được highlight bằng brand violet hoặc gradient.

Không highlight cả câu.

Ví dụ visual pattern:

**Turn Your  
Imagination  
Into Manga**

Trong đó chỉ một phrase mang brand accent.

# **18. Application Typography**

Trong workspace, typography phải yên tĩnh hơn landing page.

Suggested scale:

- Page Title: 28–32 px;

- Section Title: 20–24 px;

- Card Title: 15–18 px;

- Body: 14–16 px;

- Metadata: 12–13 px.

Workspace không được sử dụng marketing-size typography gây lãng phí không gian.

# **19. Border Radius**

Suggested radius family:

- Small: 8 px;

- Medium: 12 px;

- Large: 16 px;

- Showcase: 20–24 px.

Primary cards thường dùng 12–16 px.

Không biến toàn bộ UI thành extreme rounded-pill interface.

Pill chỉ dùng cho:

- filter;

- tags;

- status;

- compact controls.

# **20. Border Treatment**

Light Theme:

- thin;

- cool neutral;

- low contrast.

Dark Theme:

- semi-transparent blue-gray;

- chỉ đủ để phân tách layer.

Border không được cạnh tranh với content.

# **21. Shadow**

Light Theme dùng soft shadow.

Không dùng:

- large black shadow;

- strong drop shadow;

- material-style floating card everywhere.

Preferred effect:

- large blur;

- low opacity;

- subtle blue/violet tint nếu cần.

Dark Theme ưu tiên lighting và border hơn shadow.

# **22. Glass Treatment**

Glassmorphism chỉ là accent.

Phù hợp cho:

- floating navigation;

- overlay;

- hero utility;

- generation control floating panel.

Không phù hợp cho toàn bộ form/editor.

Glass surfaces phải giữ đủ contrast.

# **23. Artwork Treatment**

Artwork là visual hero của MangaCraft.

Artwork phải:

- được hiển thị ở resolution đủ lớn;

- giữ đúng aspect ratio;

- không stretch;

- có crop có chủ đích;

- không bị UI overlay che khuôn mặt nhân vật nếu tránh được.

Artwork có thể bleed ra khỏi container ở marketing surfaces.

Trong application workspace, artwork được containment rõ ràng hơn để hỗ trợ editing.

# **24. Artwork Color Priority**

UI phải nhường color richness cho artwork.

Nếu artwork rất nhiều màu:

- surrounding UI phải neutral hơn;

- không thêm quá nhiều accent.

Nếu artwork monochrome:

- brand accent có thể nổi hơn.

# **25. Illustration Direction**

Illustration marketing ưu tiên:

- manga/anime;

- cinematic composition;

- dramatic environmental lighting;

- fantasy / sci-fi / contemporary flexibility;

- rich atmosphere.

Không buộc mọi generated story phải giống anime của landing page.

Landing visual là brand language, không phải output restriction.

# **26. 3D Direction**

3D đóng vai trò hỗ trợ.

3D phù hợp với:

- floating manga panels;

- page sheets;

- project cards trong hero;

- floating creative artifacts;

- dimensional logo treatment;

- onboarding transitions.

3D không nên biến character artwork thành generic 3D render nếu sản phẩm đang quảng bá manga/anime generation.

# **27. 3D Depth Model**

Preferred 3D feeling:

**2.5D layered composition**

Thay vì một fully interactive 3D world.

Ví dụ:

- foreground paper;

- middle character artwork;

- distant castle;

- floating manga panels;

- camera parallax nhẹ.

Điều này tạo chiều sâu mà vẫn giữ illustration làm trung tâm.

# **28. Motion Personality**

Motion của MangaCraft phải có tính:

- smooth;

- elegant;

- slightly magical;

- responsive.

Không dùng:

- bounce quá mạnh;

- cartoon elasticity liên tục;

- flashy gaming transitions.

# **29. Motion Levels**

### **Level 1 — Micro Feedback**

Khoảng 120–180 ms.

Dùng cho:

- hover;

- button;

- select;

- toggle.

### **Level 2 — Component Transition**

Khoảng 180–280 ms.

Dùng cho:

- drawer;

- card;

- candidate selection;

- panel.

### **Level 3 — Creative Showcase**

Khoảng 400–800 ms.

Dùng cho:

- hero reveal;

- artwork composition;

- onboarding.

Không dùng Level 3 trong thao tác lặp đi lặp lại của editor.

# **30. Scroll Animation**

Marketing page có thể sử dụng scroll-driven animation.

Ví dụ:

- manga pages float;

- story panels chuyển từ idea thành character rồi thành world;

- background castle tạo parallax nhẹ.

Workspace application không dùng scroll animation như marketing page.

# **31. Logo Treatment**

Logo Owniverse sử dụng:

- geometric purple mark;

- clean wordmark;

- short tagline nếu layout đủ rộng.

Logo phải có Light và Dark variants.

Không thêm glow mạnh lên logo trong standard navigation.

# **32. Landing Page Visual Direction — RS-01**

## **Purpose**

Tạo nhận diện thương hiệu và giải thích ngay sản phẩm giúp user làm gì.

## **Above-the-Fold Composition**

Desktop viewport được chia theo visual weight, không phải grid cứng.

Khoảng 35–42% phía trái dành cho:

- product label;

- hero headline;

- short supporting text;

- primary CTA;

- optional secondary CTA;

- credibility metrics.

Khoảng 58–65% phía phải dành cho hero artwork.

Artwork có thể lan vào vùng giữa và nhẹ nhàng overlap text boundary nhưng không làm giảm readability.

# **33. RS-01 Hero Background**

Light Theme:

Nền bên trái gần trắng.

Đi sang phải dần hòa vào sky-blue artwork.

Không dùng hard vertical divider.

Dark Theme:

Background navy có atmospheric gradient.

Artwork vẫn sáng ở vùng focal point để character nổi bật.

# **34. RS-01 Navigation**

Navigation đặt phía trên hero.

Desktop:

- logo bên trái;

- center navigation;

- account actions bên phải.

Navigation floating, không tạo một header strip quá nặng.

Active item có violet soft background.

Primary CTA nổi bật bằng gradient hoặc saturated violet.

# **35. RS-01 Feature Strip**

Ngay dưới hero là horizontal set gồm khoảng 5–6 feature cards.

Card:

- gần square/compact landscape;

- icon phía trên;

- title;

- một dòng description;

- border nhẹ.

Các card nằm cùng một baseline.

Không dùng icon nhiều màu ngẫu nhiên.

# **36. RS-01 Story Showcase**

Section kế tiếp dùng composition giống magazine/editorial layout hơn standard SaaS section.

Bên trái:

- section label;

- large editorial heading;

- short text;

- category filters.

Bên phải:

- 3 large angled/cropped artwork panels.

Panels có thể hơi xiên để tạo energy.

Không biến section thành carousel card đồng đều nhàm chán.

# **37. RS-01 Featured Creations**

Artwork card gallery.

Mỗi card ưu tiên:

- thumbnail;

- story title;

- genre tags;

- lightweight engagement metadata nếu feature có tồn tại.

Artwork chiếm phần lớn chiều cao card.

Metadata visually secondary.

# **38. Project Dashboard — RS-02**

## **Purpose**

Đưa user từ brand experience vào creative workspace mà không gây cảm giác đổi sang một sản phẩm hoàn toàn khác.

## **Visual Direction**

Dashboard vẫn dùng MangaCraft visual identity nhưng giảm khoảng 50% decorative effects so với Landing Page.

Background:

- neutral;

- soft atmospheric accent ở header;

- không dùng full hero artwork phía sau toàn dashboard.

# **39. RS-02 Layout**

Top region gồm:

- greeting hoặc workspace title;

- short recent activity;

- Create Project CTA.

Below:

**Recent Projects**

Project cards hiển thị:

- large artwork/cover;

- project title;

- completion information;

- last edited;

- quick continue action.

Card artwork phải nổi hơn metadata.

# **40. RS-02 Project Card**

Tỷ lệ visual gần 4:3 hoặc 16:10.

Upper 60–70% là artwork.

Lower portion là metadata.

Hover:

- card nâng nhẹ;

- artwork scale rất nhỏ;

- Continue action reveal;

- border nhận subtle violet tint.

Không rotate card.

# **41. Story Workspace — RS-03**

## **Purpose**

Cho user quản lý nội dung canonical của story.

Visual character phải calm hơn generation workspace để hỗ trợ đọc và viết.

## **Layout**

Desktop sử dụng ba tầng:

**Persistent project navigation  
Story section navigation  
Content editor**

Không dùng hero artwork lớn làm background phía sau text editor.

# **42. RS-03 Story Header**

Header gồm:

- story title;

- genre/tone chips;

- current save state;

- contextual actions.

Một subtle cover artwork hoặc color accent có thể tồn tại nhưng không chiếm nhiều chiều cao.

# **43. RS-03 Content Sections**

Premise, synopsis, world information và story facts phải được chia thành structured sections.

Không dùng một textarea khổng lồ chứa toàn bộ Story Bible.

Cards/sections dùng:

- white/light surface;

- subtle border;

- generous internal padding.

AI suggestion xuất hiện như một attached assistant panel, không thay text ngay lập tức.

# **44. Character Workspace — RS-04**

## **Purpose**

Biến character thành một entity trực quan và có identity rõ ràng.

## **Desktop Composition**

Character artwork/portrait phải luôn là vùng visual focus.

Recommended composition:

- khoảng 30–35% viewport width dành cho character visual;

- khoảng 65–70% còn lại dành cho character information.

Trong viewport nhỏ hơn, portrait chuyển lên trên.

# **45. RS-04 Character Portrait**

Portrait:

- large;

- near full-height card;

- neutral/background gradient;

- minimal UI overlay.

Canonical reference có clear badge:

**Current Reference**

Alternative images nằm trong thumbnail strip hoặc reference gallery bên dưới.

# **46. RS-04 Character Data**

Character information chia bằng tabs hoặc segmented sections:

- Profile;

- Identity;

- Appearance;

- References;

- Story Usage.

Identity fields có lock control.

Locked field không trở thành visually disabled.

User vẫn phải hiểu nó là editable bởi user nhưng protected khỏi AI overwrite.

# **47. RS-04 Character Identity Visual Language**

Stable identity section có visual weight lớn hơn mutable appearance section.

Không dùng màu đỏ/cảnh báo cho identity.

Có thể dùng small shield/lock icon kết hợp neutral brand accent.

# **48. Scene Workspace — RS-05**

## **Purpose**

Đây là **reference application screen quan trọng nhất** của toàn bộ Owniverse.

Scene Workspace phải thể hiện rõ triết lý:

> Story information ở một phía.  
> Generated visual ở phía còn lại.  
> AI kết nối hai phía nhưng không thay thế Story.

# **49. RS-05 Main Layout**

Desktop application dùng persistent project sidebar.

Main workspace chia thành hai vùng lớn.

### **Scene Editor Region**

Khoảng 40–45% available width.

Chứa:

- scene title;

- scene description;

- setting;

- active characters;

- scene-specific character states;

- action;

- emotion;

- optional generation instruction.

### **Visual Region**

Khoảng 55–60%.

Chứa:

- selected image lớn;

- generation status;

- consistency status;

- candidates;

- Generate / Regenerate actions.

Image side được phép visually dominant.

# **50. RS-05 Selected Artwork**

Selected artwork phải là object lớn nhất trong screen.

Artwork được đặt trong clean image viewport với:

- dark hoặc neutral inspection background tùy theme;

- rounded frame vừa phải;

- không quá nhiều overlay.

Metadata/controls nằm bên cạnh hoặc bên dưới, không che artwork trừ action tạm thời.

# **51. RS-05 Candidate Strip**

Candidates nằm dưới selected artwork hoặc trong horizontal strip.

Thumbnail đủ lớn để so sánh character identity.

Selected candidate:

- clear border;

- check indicator;

- label.

Warning candidate:

- warning badge;

- không dùng full red border nếu chỉ là warning.

# **52. RS-05 Character Chips**

Characters in scene không nên chỉ là tên text.

Dùng compact chips/card gồm:

- small avatar;

- character name;

- state summary.

Click mở scene-specific character state.

# **53. RS-05 Generation Action**

Generate Image là primary CTA khi scene chưa có visual.

Sau khi đã có image:

- Regenerate trở thành primary contextual AI action;

- selected image không bị mất.

Advanced generation options được đặt trong drawer hoặc collapsed area.

# **54. Generation State — RS-06**

## **Purpose**

Biến thời gian chờ AI thành một trạng thái dễ hiểu thay vì spinner không xác định.

## **Visual Behavior**

Image region giữ nguyên kích thước để tránh layout shift.

Trong image viewport:

- subtle animated gradient;

- blurred creative preview background nếu có;

- generation stage;

- progress indicator không cần giả số phần trăm nếu backend không cung cấp.

# **55. RS-06 Copy**

User-facing states:

**Preparing your scene**

**Creating artwork**

**Checking character consistency**

**Finishing image**

Không hiển thị tên diffusion pipeline ở default mode.

# **56. RS-06 Navigation Behavior**

Generation không block workspace.

User có thể chuyển scene hoặc character.

Sidebar hoặc Scene list có small running indicator.

Khi job hoàn thành, thông báo nhẹ được gửi.

# **57. Candidate Review — RS-07**

## **Purpose**

Cho user đánh giá nhiều output bằng mắt và chọn kết quả phù hợp.

## **Layout**

Có hai modes.

### **Grid Review**

Dùng khi có nhiều candidates.

Các image có kích thước gần bằng nhau.

### **Compare Mode**

Dùng khi user chọn 2 candidates.

Hai artwork đặt side-by-side với synchronized zoom nếu implementation hỗ trợ.

# **58. RS-07 Candidate Metadata**

Metadata không nằm ở trung tâm visual hierarchy.

Có thể gồm:

- created time;

- validation state;

- consistency warning;

- user instruction;

- generation attempt.

Technical parameters nằm trong expandable details.

# **59. RS-07 Validation Treatment**

Candidate đạt:

**Looks consistent**

Candidate cần review:

**Needs Review**

Expanded detail có thể giải thích:

- possible hairstyle difference;

- possible facial identity drift;

- scene alignment issue.

Không dùng raw research metric làm primary user-facing text.

# **60. Generation History — RS-08**

## **Purpose**

Cho user hiểu evolution của Scene mà không biến màn hình thành technical log.

## **Visual Model**

History theo chronological generation groups.

Mỗi attempt hiển thị:

- timestamp;

- short instruction;

- candidate thumbnail;

- status;

- selected marker.

# **61. RS-08 Timeline**

Timeline visually nhẹ.

Không dùng enterprise audit-log table làm default view.

Artwork thumbnail là anchor chính.

Technical metadata chỉ hiện khi expand.

# **62. Light/Dark Theme Mapping**

Light và Dark Theme phải giữ:

- cùng spacing;

- cùng hierarchy;

- cùng typography;

- cùng component geometry.

Nhưng lighting khác nhau.

Light:

- shadow tạo depth;

- color subtle;

- glow hạn chế.

Dark:

- borders và local light tạo depth;

- glow rõ hơn;

- image luminance được tận dụng.

# **63. Theme Transition**

Nếu hỗ trợ theme switch, transition phải ngắn.

Không animate toàn page bằng dramatic color morph kéo dài.

Artwork không bị filter chỉ để phù hợp theme.

# **64. AI Visual Signature**

AI-related action sử dụng một visual signature nhất quán.

Có thể gồm:

- small sparkle icon;

- violet-blue accent;

- gradient trong primary AI CTA;

- subtle animated light trong processing state.

Không dùng sparkle icon cho mọi component.

Nếu mọi thứ đều trông “AI”, signature sẽ mất ý nghĩa.

# **65. Manual Action vs AI Action**

Manual action:

- neutral hoặc standard brand styling.

AI action:

- subtle differentiated treatment.

Ví dụ:

**Save Scene** → standard primary/secondary button.

**Generate Image** → brand gradient hoặc AI-accent button.

# **66. Empty State Direction**

Empty state là nơi có thể expressive hơn editor.

Ví dụ Characters Empty State:

Center area có một floating character silhouette hoặc layered manga sheets.

Text:

- strong short title;

- one sentence;

- primary manual action;

- secondary AI action.

Không hiển thị giant technical illustration về neural network.

# **67. Error State Direction**

Error visual phải calm.

AI generation failure:

- image area vẫn giữ layout;

- error icon vừa phải;

- concise message;

- Retry action.

Không chuyển cả màn hình sang màu đỏ.

# **68. Warning Direction**

Warning dùng amber/yellow semantic accent.

Consistency warning phải xuất hiện gần artwork hoặc candidate liên quan.

Không hiển thị global warning banner nếu chỉ một candidate có vấn đề.

# **69. Loading Direction**

Generic content loading:

Skeleton.

AI generation:

Creative generation state riêng.

Không dùng skeleton giống nhau cho hai trường hợp.

# **70. Responsive Visual Behavior**

## **Desktop ≥ approximately 1280 px**

Full creative workspace.

Sidebar + split editor.

## **Medium**

Sidebar có thể collapse.

Split areas resize.

## **Tablet**

Scene editor và visual region chuyển sang tabs hoặc stacked panels.

## **Mobile**

Review-oriented.

Scene editing cơ bản.

Không ép desktop editor vào một viewport hẹp bằng cách thu nhỏ toàn bộ UI.

# **71. Marketing vs Application Visual Density**

Marketing surfaces:

- large typography;

- whitespace;

- illustration;

- 3D/depth;

- atmospheric visuals.

Application surfaces:

- smaller typography;

- denser information;

- restrained effects;

- persistent navigation;

- interaction clarity.

Hai nhóm phải chung brand DNA nhưng không dùng cùng density.

# **72. Do — Visual Rules**

Developer/designer SHOULD:

- ưu tiên artwork;

- sử dụng neutral surfaces;

- dùng violet làm brand anchor;

- giữ whitespace;

- tạo depth bằng layers;

- giữ border nhẹ;

- dùng animation có mục đích;

- dùng serif cho expressive marketing headline;

- dùng sans-serif cho workspace;

- giữ AI controls secondary với creative content;

- duy trì cùng visual DNA ở cả light và dark.

# **73. Don't — Visual Rules**

Developer/designer SHALL NOT:

- biến app thành cyberpunk dashboard;

- dùng neon border cho mọi card;

- dùng gradient trên mọi button;

- dùng glass effect trên mọi surface;

- dùng 3D trong dense editor chỉ để trang trí;

- để AI controls che artwork;

- sử dụng pure black làm main Dark Theme background;

- dùng pure white text ở mọi hierarchy;

- tạo quá nhiều card nested trong card;

- để mỗi screen có một visual style riêng;

- copy visual branding của secondary references;

- tự thay palette chỉ vì một generated image có màu khác.

# **74. External Reference Policy**

Secondary Internet Reference chỉ được thêm khi nó minh họa một vấn đề cụ thể.

Mỗi reference phải có metadata:

**Reference ID  
Source/Product  
Purpose  
Borrow  
Do Not Copy**

Ví dụ:

> Borrow: image comparison interaction.  
> Do Not Copy: colors, branding, typography, page architecture.

Secondary references không được thay đổi visual identity đã khóa bởi VR-01 và VR-02.

# **75. Mockup Policy**

Mỗi reference screen khi có mockup chính thức sẽ được đưa vào UX-03 với ID:

- RM-01;

- RM-02;

- ...

Mockup sau khi approved trở thành visual reference cao hơn implementation tự phát.

# **76. Reference Screen Documentation Rule**

Mỗi mockup phải có:

- screen ID;

- viewport/reference size;

- purpose;

- layout breakdown;

- component mapping;

- interaction notes;

- animation notes;

- light/dark state nếu có;

- responsive behavior;

- known deviations.

# **77. Developer Handoff Rule**

Frontend không được implement chỉ bằng cách “nhìn giống hình”.

Mỗi screen phải được dựng từ:

**UX-01 flow**

- **UX-02 component rules**

- **UX-03 visual specification**

Visual reference chỉ cung cấp final composition direction.

# **78. Design Token Traceability**

Các color, spacing và component values được implementation dưới dạng tokens.

Ví dụ:

brand.primary

brand.secondary

background.canvas

surface.default

surface.elevated

text.primary

text.secondary

border.subtle

shadow.card

radius.card

motion.fast

Không hard-code cùng một visual meaning bằng nhiều giá trị rời rạc ở nhiều page.

# **79. Accessibility Constraint**

Visual richness không được phá accessibility.

Implementation phải kiểm tra:

- contrast;

- keyboard focus;

- reduced motion;

- text readability;

- status differentiation.

Một màu đẹp trong mockup không được giữ nếu contrast thực tế không đạt yêu cầu.

Trong trường hợp đó phải điều chỉnh brightness/lightness trong cùng hue family.

# **80. Performance Constraint**

Các visual effect như:

- blur;

- parallax;

- video;

- WebGL;

- 3D;

- particles;

không được trở thành requirement bắt buộc của core workspace.

Nếu thiết bị yếu:

UI phải degrade về static visual mà vẫn giữ hierarchy và usability.

# **81. Primary Reference Summary**

VR-01 và VR-02 định nghĩa ba điểm không được mất khi sản phẩm phát triển.

### **1. Artwork Dominance**

Artwork luôn mạnh hơn UI chrome.

### **2. Violet Creative Identity**

Violet/blue tạo identity xuyên suốt sản phẩm.

### **3. Editorial + Creative Software Fusion**

MangaCraft không hoàn toàn là gallery và cũng không hoàn toàn là productivity app.

Nó kết hợp:

**editorial storytelling aesthetics**

với

**structured creative workspace**.

# **82. Acceptance Criteria**

## **AC-UX03-01**

Một developer chỉ đọc UX-03 mà không xem VR-01/VR-02 vẫn phải hiểu visual direction chính của Owniverse.

## **AC-UX03-02**

Light Theme và Dark Theme phải được nhận biết là cùng một sản phẩm.

## **AC-UX03-03**

Artwork phải là visual focus trên Character, Scene và Candidate screens.

## **AC-UX03-04**

Primary brand family phải giữ violet/indigo/blue direction.

## **AC-UX03-05**

Application workspace không được sử dụng cùng mức decorative complexity của Landing Page.

## **AC-UX03-06**

AI action phải nhận biết được nhưng không dominate manual creative workflow.

## **AC-UX03-07**

Scene Workspace phải phân biệt rõ scene content và generated visual.

## **AC-UX03-08**

Visual specification không được phụ thuộc hoàn toàn vào reference image.

## **AC-UX03-09**

3D và animation không được làm core workflow unusable khi chúng bị disable.

## **AC-UX03-10**

Mockup mới phải tuân thủ VR-01/VR-02 và UX-03 trừ khi visual direction chính thức được review lại.

# **83. Relationship of UX Documents**

**UX-01 — Information Architecture & UX Flow Specification**

Xác định:

- user đi đâu;

- screen nào tồn tại;

- workflow diễn ra như thế nào.

**UX-02 — UI Design System & Interaction Specification**

Xác định:

- component hoạt động như thế nào;

- spacing;

- controls;

- interaction;

- UI states;

- accessibility behavior.

**UX-03 — Visual Direction & Reference Screen Specification**

Xác định:

- sản phẩm trông như thế nào;

- màu sắc;

- visual personality;

- artwork treatment;

- lighting;

- 3D;

- motion personality;

- các màn hình chuẩn phải được compose ra sao.

Ba tài liệu kết hợp tạo thành specification hoàn chỉnh cho UX/UI của Owniverse.

# **84. Final Visual Principle**

Visual principle cuối cùng của Owniverse:

> **The interface should feel like the doorway into the user's story world, not the machinery that generated it.**

AI, model và generation infrastructure tồn tại phía sau.

Những gì user nhìn thấy ở phía trước phải là:

**ý tưởng → nhân vật → scene → artwork → thế giới của câu chuyện.**
