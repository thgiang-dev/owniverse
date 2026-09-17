# **AI-02 — Context, Memory & Consistency Specification**

**System:** AI Story Creation Studio  
**Document ID:** AI-02  
**Document Title:** Context, Memory & Narrative Consistency Specification  
**Version:** 0.1  
**Status:** Draft for Review  
**Document Type:** AI System / Domain Intelligence Specification  
**Primary Owner:** AI Systems Architect / Domain Architect  
**Primary Audience:** AI Engineer, Backend Developer, Database Engineer, QA Engineer, Product Owner, Researcher, Thesis Supervisor

## **1. Purpose**

Tài liệu này đặc tả cách hệ thống duy trì và sử dụng thông tin của một câu chuyện dài nhằm hỗ trợ AI tạo nội dung mới mà vẫn nhất quán với nội dung đã được người dùng xác nhận trước đó.

Tài liệu giải quyết ba vấn đề trung tâm:

1.  **Context Management:** AI cần nhận thông tin nào khi thực hiện một tác vụ cụ thể?

2.  **Long-term Story Memory:** hệ thống lưu và truy xuất thông tin dài hạn của câu chuyện như thế nào?

3.  **Narrative Consistency:** hệ thống phát hiện nội dung mới mâu thuẫn với Canon, Character State, Timeline hoặc Character Knowledge như thế nào?

Đây là thành phần trực tiếp hỗ trợ mục tiêu nghiên cứu:

> **Character-Consistent Long-Range Story Generation**

# **2. Relationship to Other Documents**

AI-02 nằm giữa Story Core và Generation Pipeline.

SC — Story Core Specification

→ Xác định dữ liệu nào tồn tại và đâu là Canon.

↓

AI-02 — Context, Memory & Consistency

→ Xác định dữ liệu nào cần được lấy ra,

tổ chức và kiểm tra tại thời điểm generation.

↓

AI-03 — Generation Pipeline

→ Sử dụng Context Package để sinh Scene,

dialogue, storyboard và image.

Nói cách khác:

**Story Core lưu sự thật.  
AI-02 chọn đúng sự thật cần dùng.  
AI-03 sử dụng chúng để tạo nội dung.**

# **3. Scope**

AI-02 bao gồm:

| **Area**               | **Responsibility**                               |
|------------------------|--------------------------------------------------|
| Context Retrieval      | Tìm dữ liệu liên quan đến request hiện tại       |
| Context Assembly       | Tổ chức dữ liệu thành Context Package            |
| Context Prioritization | Xác định thông tin nào quan trọng hơn            |
| Long-term Memory       | Lưu thông tin dài hạn ở dạng phù hợp truy xuất   |
| Memory Extraction      | Rút Fact/Event/State từ Canon                    |
| Character State        | Cung cấp trạng thái hiện tại của nhân vật        |
| Character Knowledge    | Phân biệt điều nhân vật biết với điều Story biết |
| Relationship Context   | Theo dõi quan hệ thay đổi theo thời gian         |
| Timeline Context       | Duy trì thứ tự Event                             |
| Consistency Checking   | Phát hiện contradiction                          |
| Impact Analysis        | Xác định ảnh hưởng khi Canon cũ thay đổi         |

AI-02 không đặc tả chi tiết:

- model cụ thể;

- embedding model cụ thể;

- prompt wording cụ thể;

- text generation algorithm;

- image generation algorithm;

- evaluation methodology của nghiên cứu.

Những phần đó có thể thay đổi mà không làm thay đổi contract được định nghĩa tại đây.

# **4. Normative Language**

Trong tài liệu này:

| **Term**     | **Ý nghĩa**                                |
|--------------|--------------------------------------------|
| **SHALL**    | Bắt buộc phải thực hiện                    |
| **SHOULD**   | Nên thực hiện, chỉ bỏ khi có lý do rõ ràng |
| **MAY**      | Tùy chọn                                   |
| **MUST NOT** | Không được phép                            |

Cách viết này giúp developer và QA phân biệt requirement bắt buộc với recommendation.

# **5. Terminology**

## **Canon**

Thông tin đã được người dùng xác nhận là sự thật chính thức trong một Story Branch.

Ví dụ:

> Aria lost her sword in Chapter 12.

## **Story Truth**

Sự thật mà hệ thống biết về thế giới truyện.

Ví dụ:

> Kael secretly works for the King.

Story Truth không đồng nghĩa với việc tất cả Character đều biết thông tin đó.

## **Character Knowledge**

Những thông tin mà một Character cụ thể đã biết tại một thời điểm cụ thể.

Ví dụ:

Story Truth

→ Kael works for the King.

Kael knows this

→ Yes.

Aria knows this

→ No.

## **Character State**

Trạng thái hiện tại của Character.

Ví dụ:

Location → Central Station

Physical → Injured left arm

Possession → No sword

Goal → Reach Eldoria

Outfit → Winter coat

Status → Alive

## **Memory**

Dữ liệu được tổ chức nhằm giúp hệ thống nhớ lại những thông tin đã xảy ra trước đó mà không cần gửi toàn bộ câu chuyện cho AI.

## **Context**

Tập hợp thông tin được chọn cho **một AI task cụ thể**.

Memory là dữ liệu được lưu lâu dài.

Context là dữ liệu được chọn từ Memory và Story Core cho request hiện tại.

Do đó:

> **Memory ≠ Context.**

## **Context Package**

Object có cấu trúc chứa toàn bộ thông tin sẽ được chuyển cho Generation Pipeline.

# **6. Core Design Principle**

Hệ thống không được dựa vào mô hình AI để tự nhớ câu chuyện.

Thay vào đó:

Story Core / Canon

→ Nguồn sự thật lâu dài.

Memory Layer

→ Dữ liệu đã được tổ chức để truy xuất hiệu quả.

Context Retrieval

→ Chọn những thông tin liên quan đến task hiện tại.

Context Package

→ Gói dữ liệu được đưa cho AI.

Model

→ Tạo nội dung dựa trên Context Package.

### **Giải thích**

Nếu Chapter 100 cần nhắc lại một sự kiện ở Chapter 3, hệ thống không nên gửi toàn bộ Chapter 1–99 vào model.

Hệ thống phải tìm được chính xác Event hoặc Fact của Chapter 3 liên quan tới Scene hiện tại và đưa nó vào Context Package.

Đây chính là vai trò của Context Retrieval.

# **7. Context Architecture**

Kiến trúc logic của Context Engine:

┌──────────────────────┐

│ Generation Request │

│ → user muốn tạo gì │

└──────────┬───────────┘

│

▼

┌──────────────────────┐

│ Request Interpreter │

│ → xác định task, │

│ entity và scope │

└──────────┬───────────┘

│

┌────────────────┼─────────────────┐

▼ ▼ ▼

Story Core Memory Store Recent Context

→ Canon → facts → scenes gần

→ states → summaries

→ rules → embeddings

│ │ │

└────────────────┼─────────────────┘

▼

┌──────────────────────┐

│ Context Retriever │

│ → tìm candidate │

└──────────┬───────────┘

▼

┌──────────────────────┐

│ Context Ranker │

│ → ưu tiên dữ liệu │

└──────────┬───────────┘

▼

┌──────────────────────┐

│ Context Assembler │

│ → xây package cuối │

└──────────┬───────────┘

▼

┌──────────────────────┐

│ Consistency Guard │

│ → kiểm tra conflict │

└──────────┬───────────┘

▼

Context Package

### **Cách đọc sơ đồ**

Request Interpreter trước tiên phải hiểu user đang muốn làm gì.

Ví dụ:

> “Viết Scene tiếp theo, nơi Aria gặp lại Kael.”

Hệ thống có thể xác định:

Task

→ SCENE_GENERATION

Primary Characters

→ Aria, Kael

Current Chapter

→ Chapter 24

Current Branch

→ Main

Relevant Relationship

→ Aria ↔ Kael

Likely Historical Topics

→ betrayal, trust, royal secret

Từ đó Retriever mới tìm dữ liệu phù hợp.

# **8. Context Source Categories**

Context Package được xây dựng từ nhiều loại nguồn khác nhau.

## **8.1 User Instruction**

Đây là creative instruction hiện tại của user, nhưng **không có quyền tự động ghi đè Canon hoặc locked identity/state**.

Ví dụ:

> “Cho Aria tha thứ Kael trong scene này.”

Hệ thống phải hiểu đây là **desired future direction**, không phải Canon hiện tại.

Nếu instruction xung đột với locked Canon, AI layer phải giữ Canon làm constraint và không âm thầm sửa Canon trong generation.

Priority chung của generation context được khóa theo thứ tự:

1.  **Canonical / Locked Constraints** — Story Rules, locked Character identity và dữ liệu Canon bắt buộc;

2.  **Timeline-valid / Scene State** — trạng thái Character, Relationship, Scene và World có hiệu lực tại target Scene;

3.  **User Instruction** — creative direction hợp lệ trong phạm vi không xung đột với hai lớp trên;

4.  **Retrieved / Soft Context** — historical references, summaries, examples và các nguồn hỗ trợ;

5.  **Model Assumption** — chỉ được sử dụng khi các lớp trên không cung cấp thông tin.

Có thể nhớ ngắn gọn:

> **Canon / Locked Identity \> Scene State \> User Instruction \> Soft Context \> Model Assumption**

## **8.2 Story Rules**

Các quy tắc có scope toàn truyện.

Ví dụ:

> Magic does not exist in this world.

Story Rule được xem như constraint.

## **8.3 Character Identity**

Thông tin tương đối ổn định:

- personality;

- background;

- core motivation;

- visual identity;

- speaking style.

## **8.4 Character Current State**

Thông tin hiện tại tại điểm timeline đang generation.

Ví dụ:

Aria

Location → Central Station

Injury → Left arm injured

Possession → No sword

Knowledge → Knows Kael was threatened

Goal → Find brother

## **8.5 Relationship State**

Trạng thái hiện tại giữa các Character liên quan.

## **8.6 Recent Narrative Context**

Một số Scene hoặc Chapter gần nhất.

Recent context đặc biệt quan trọng để đảm bảo continuity cục bộ.

## **8.7 Historical Canon Facts**

Những Fact cũ nhưng có liên quan tới Scene hiện tại.

## **8.8 Historical Events**

Các Event quan trọng trong Timeline.

## **8.9 Character Knowledge**

Thông tin mà từng Character được phép biết.

## **8.10 Custom Rules and References**

Các rule do user thêm như:

- Dialogue Rule;

- Fighting Style;

- Character Arc Plan;

- Writing Style;

- custom lore.

# **9. Mandatory Context vs Retrieved Context**

Không phải context nào cũng cần semantic search.

Hệ thống chia dữ liệu thành hai nhóm.

### **Mandatory Context**

Luôn được lấy trực tiếp từ domain model nếu applicable.

Ví dụ:

Active Story Rules

Current Scene

Character Current State

Current Relationship State

Branch

User Instruction

### **Retrieved Context**

Chỉ được lấy khi liên quan.

Ví dụ:

Old Events

Old Canon Facts

Old Chapter Summaries

Lore

Past dialogue

Historical relationship changes

Thiết kế này ngăn một lỗi phổ biến:

> sử dụng vector search cho mọi thứ, kể cả dữ liệu mà hệ thống đã biết chính xác vị trí.

# **10. Retrieval Pipeline**

Context retrieval được thực hiện theo nhiều bước.

1\. Resolve Scope

→ Project, Branch, Chapter, Scene, Character.

2\. Load Mandatory Context

→ Rules và current states.

3\. Build Retrieval Query

→ Dựa trên user request + current scene.

4\. Retrieve Candidates

→ Tìm Fact/Event/Summary/Reference liên quan.

5\. Filter

→ Loại Draft, branch khác, data hết hiệu lực.

6\. Rank

→ Xếp hạng mức liên quan.

7\. Deduplicate

→ Loại thông tin trùng lặp.

8\. Apply Context Budget

→ Chỉ giữ lượng thông tin cần thiết.

9\. Assemble

→ Tạo Context Package.

# **11. Candidate Retrieval**

V1 có thể kết hợp hai hình thức retrieval.

### **Structured Retrieval**

Dựa trên relation trực tiếp trong database.

Ví dụ:

Scene → Character IDs

Character → Current State

Character Pair → Relationship

Scene → Location

Branch → Timeline

Đây là phương pháp có độ chính xác cao.

### **Semantic Retrieval**

Dựa trên embedding/vector similarity để tìm những thông tin liên quan về nội dung.

Ví dụ user yêu cầu:

> “Aria bắt đầu nghi ngờ Kael một lần nữa.”

Semantic search có thể tìm:

- sự kiện Kael từng phản bội;

- lời hứa trước đây;

- Chapter có conflict liên quan trust.

PostgreSQL + pgvector có thể được sử dụng cho lớp này.

# **12. Hybrid Retrieval**

Hệ thống SHOULD sử dụng **hybrid retrieval** thay vì vector search đơn thuần.

Structured Retrieval

→ “Ai? Ở đâu? State hiện tại là gì?”

\+

Semantic Retrieval

→ “Sự kiện cũ nào có ý nghĩa với scene này?”

\+

Temporal Filtering

→ “Thông tin này đã xảy ra trước thời điểm hiện tại chưa?”

\+

Branch Filtering

→ “Thông tin này có thuộc timeline hiện tại không?”

Kết quả là một candidate set đáng tin cậy hơn.

# **13. Context Ranking**

Candidate có thể được xếp hạng dựa trên nhiều tín hiệu.

Một formulation khái niệm:

Score(x)=wsSsemantic+weSentity+wtStemporal+wpSpriority+wrSrecencyScore(x) = w_s S\_{semantic} + w_e S\_{entity} + w_t S\_{temporal} + w_p S\_{priority} + w_r S\_{recency}

Trong đó:

| **Thành phần**         | **Ý nghĩa**                                   |
|------------------------|-----------------------------------------------|
| SsemanticS\_{semantic} | Mức liên quan về nội dung                     |
| SentityS\_{entity}     | Mức liên quan tới Character/Location hiện tại |
| StemporalS\_{temporal} | Phù hợp với timeline                          |
| SpriorityS\_{priority} | Priority metadata                             |
| SrecencyS\_{recency}   | Mức gần với Scene hiện tại                    |

Các trọng số ww không phải business rule cố định.

Chúng có thể được điều chỉnh hoặc trở thành một phần của thí nghiệm nghiên cứu.

### **Yêu cầu kiến trúc**

Backend và AI Service không được phụ thuộc vào một công thức ranking duy nhất.

Ranking Strategy phải có khả năng thay thế.

# **14. Context Priority Policy**

Nếu hai nguồn thông tin conflict, hệ thống sử dụng hierarchy:

1\. Explicit Canon Correction

2\. Active Story Rule

3\. Current Canon State

4\. Canon Event / Canon Fact

5\. Approved Custom Rule

6\. Approved Summary

7\. Reference Material

8\. AI-generated Draft

Draft không được phép override Canon.

# **15. Temporal Validity**

Một Fact không nhất thiết đúng mãi mãi.

Ví dụ:

Chapter 1

Aria owns Sword.

Chapter 12

Sword is destroyed.

Không thể chỉ lưu:

Aria owns Sword.

Fact phải hỗ trợ effective interval.

Ví dụ:

Fact

Aria owns Silver Sword

Valid From

Chapter 03 / Scene 02

Valid Until

Chapter 12 / Scene 03

Khi generate Chapter 20, Fact này không còn active.

# **16. Branch Isolation**

Tất cả retrieval SHALL giới hạn theo Branch.

Ví dụ:

Main Branch

Chapter 20

→ Kael survives.

Alternative Branch

Chapter 20

→ Kael dies.

Khi đang generation Main Branch, dữ liệu:

> Kael died

từ Alternative Branch không được xuất hiện trong Context Package.

**CTX-BR-001  
**Cross-branch retrieval MUST NOT occur unless explicitly requested by the user.

# **17. Story Memory Architecture**

Memory không phải một text block duy nhất.

Kiến trúc memory:

Story Memory

│

├── Global Memory

│ → Premise, world facts, permanent rules.

│

├── Character Memory

│ → Identity, state, knowledge, relevant history.

│

├── Relationship Memory

│ → Current relationship + important transitions.

│

├── Event Memory

│ → Canon Events.

│

├── Canon Fact Memory

│ → Structured atomic facts.

│

├── Chapter Summary

│ → Summary theo chapter.

│

├── Arc Summary

│ → Summary phạm vi nhiều chapter.

│

└── Recent Narrative Context

→ Scene gần nhất ở dạng chi tiết hơn.

Mỗi lớp có mục đích khác nhau.

# **18. Why Hierarchical Memory Is Required**

Giả sử một story có 100 Chapter.

Nếu chỉ lưu Chapter text:

- retrieval tốn token;

- thông tin quan trọng bị chôn trong prose;

- khó biết state nào còn hiệu lực.

Nếu chỉ lưu summary:

- mất chi tiết;

- khó phục hồi Fact cụ thể.

Nếu chỉ lưu vector:

- khó xác định truth;

- vector similarity không xử lý tốt state transition.

Do đó hệ thống cần kết hợp:

> **structured facts + summaries + states + semantic retrieval.**

# **19. Memory Creation Lifecycle**

Memory chỉ được cập nhật từ Canon.

Draft

→ User chỉnh nội dung.

↓

Approve

→ Scene trở thành Canon.

↓

Memory Extraction

→ AI phát hiện Fact/Event/State/Relationship change.

↓

Structured Validation

→ Kiểm tra entity và transition.

↓

Domain Commit

→ Backend cập nhật Canon structures.

↓

Embedding / Indexing

→ Dữ liệu có thể được index phục vụ retrieval.

↓

Memory Available

→ Generation sau có thể sử dụng.

### **Điểm quan trọng**

Memory Extraction không được coi là truth ngay khi AI trả về.

AI chỉ đề xuất extraction.

Backend phải validate trước khi commit.

# **20. Memory Extraction Output**

Một extraction operation có thể trả về:

{

"events": \[\],

"canonFacts": \[\],

"characterStateChanges": \[\],

"characterKnowledgeChanges": \[\],

"relationshipChanges": \[\]

}

Schema chi tiết được thiết kế ở data/API layer, nhưng semantic meaning phải tuân theo specification này.

# **21. Canon Fact Model**

Canonical Fact nên càng atomic càng tốt.

Ví dụ không nên lưu:

> “Aria fought Kael at the station, lost her sword and now hates him.”

Nên tách thành:

Fact 1

Aria fought Kael.

Fact 2

Aria lost Silver Sword.

Fact 3

Aria's trust toward Kael decreased.

Việc tách nhỏ giúp retrieval và consistency check chính xác hơn.

# **22. Character State Management**

Character State là **snapshot logic hiện tại**, không phải summary toàn bộ lịch sử.

Ví dụ:

Character: Aria

Current State

├── Location → Central Station

├── Physical → Left arm injured

├── Possessions → Map, Ring

├── Knowledge → Kael was threatened

├── Objective → Reach Eldoria

├── Outfit → Winter coat

└── Status → Alive

State History lưu các transition dẫn đến Current State.

# **23. State Transition**

Một state change phải có nguồn.

BEFORE

Aria.location = Blackwood Forest

EVENT

Aria boards the train.

AFTER

Aria.location = Central Station

Logical representation:

StateTransition

→ Entity: Aria

→ Property: location

→ OldValue: Blackwood Forest

→ NewValue: Central Station

→ Source: Event EV-239

# **24. State Update Policy**

**MEM-ST-001  
**Character State SHALL NOT be updated from an unapproved draft.

**MEM-ST-002  
**Every automatically extracted state transition SHALL reference its Canon source.

**MEM-ST-003  
**Current State SHALL be branch-specific.

**MEM-ST-004  
**Previous State SHALL remain recoverable through State History.

# **25. Character Knowledge Model**

Một trong những nguyên nhân phổ biến gây story inconsistency là Character biết những thứ mà họ chưa từng được biết.

Do đó system truth và character knowledge phải tách riêng.

Ví dụ:

Story Fact

→ The masked assassin is Kael.

Kael Knowledge

→ Knows.

King Knowledge

→ Knows.

Aria Knowledge

→ Does not know.

Reader Knowledge

→ Depends on narrative reveal.

# **26. Knowledge Acquisition**

Character Knowledge thay đổi khi một Event tạo điều kiện cho Character biết thông tin.

Ví dụ:

Scene

→ Aria overhears Kael speaking to the King.

Event

→ Aria discovers Kael's affiliation.

Knowledge Transition

Aria knows "Kael works for the King"

False → True

# **27. Knowledge Consistency Rule**

**KNOW-001**

A Character MUST NOT act upon Canon information that is not present in the Character's accessible knowledge state, unless the narrative explicitly represents the action as speculation, intuition, deception, or another justified mechanism.

Điều này cho phép hệ thống phân biệt:

**Sai consistency**

> Aria nói chính xác bí mật chưa từng biết.

với

**Hợp lệ**

> Aria nghi ngờ Kael đang giấu điều gì đó.

# **28. Relationship Memory**

Relationship phải chứa cả state hiện tại và lịch sử thay đổi.

Ví dụ:

Aria ↔ Kael

Chapter 01

→ Stranger

Chapter 06

→ Ally

Chapter 12

→ Close Friend

Chapter 20

→ Distrust

Chapter 24

→ Cautious Cooperation

Current relationship tại Chapter 24 là:

Cautious Cooperation

nhưng historical events vẫn có thể được retrieval nếu Scene đề cập betrayal/trust.

# **29. Summary Hierarchy**

Đối với truyện dài, summary được tạo theo nhiều cấp.

Scene Summary

→ Tóm tắt một Scene.

Chapter Summary

→ Tổng hợp các Scene trong Chapter.

Arc Summary

→ Tổng hợp một nhóm Chapter.

Story Summary

→ Tóm tắt trạng thái chung toàn truyện.

Summary cấp cao giúp model hiểu bối cảnh rộng.

Fact/Event giúp model truy cập chi tiết chính xác.

# **30. Context Budget Management**

Mỗi AI model có giới hạn context.

Do đó không thể đưa tất cả candidate vào prompt.

Context Assembler phải phân bổ budget.

Ví dụ conceptual:

Total Context Budget

│

├── System / Output Rules

├── User Instruction

├── Story Rules

├── Character Current State

├── Current Scene / Chapter

├── Recent Context

├── Retrieved Historical Memory

└── Optional References

Thông tin bắt buộc được giữ trước.

Thông tin historical có relevance thấp được loại trước.

# **31. Context Compression**

Nếu context vẫn quá lớn, hệ thống MAY:

- sử dụng summary;

- loại duplicate;

- thay nhiều Scene bằng Chapter Summary;

- chọn Event thay vì full prose;

- rút ngắn low-priority reference.

Hệ thống MUST NOT tự cắt mất Critical Story Rule để tiết kiệm context.

# **32. Context Package / GenerationContextV1 Contract**

Context Package là abstraction giữa AI-02 và AI-03.

Đối với generation runtime, contract chuẩn đầu tiên được khóa là:

> **GenerationContextV1**

Raw prompt string **không phải** contract chính giữa Application và AI layer. Application xây structured context trước; từng Adapter chịu trách nhiệm biến structured context thành model-specific prompt/conditioning/input.

Logical structure:

{

"schemaVersion": "1.0",

"request": {

"operationType": "IMAGE_GENERATION",

"targetType": "SCENE",

"targetId": "..."

},

"story": {

"storyCoreVersion": 7,

"premise": "...",

"summary": "...",

"style": "..."

},

"scene": {

"sceneId": "...",

"sceneVersion": 4,

"description": "...",

"intent": "...",

"location": "...",

"mood": "..."

},

"characters": \[

{

"characterId": "...",

"characterVersion": 3,

"stableIdentity": {},

"references": \["asset-id"\],

"sceneState": {

"outfit": "...",

"emotion": "...",

"action": "...",

"temporaryAppearance": "..."

}

}

\],

"relationships": \[\],

"continuity": {

"recentContext": \[\],

"relevantEvents": \[\],

"relevantFacts": \[\]

},

"userInstruction": "...",

"generationOptions": {},

"provenance": {}

}

Các operation khác có thể sử dụng subset hoặc extension phù hợp nhưng vẫn phải giữ schemaVersion và semantic boundary của structured context.

AI-03 chỉ cần biết cách sử dụng GenerationContextV1 hoặc version kế tiếp của contract.

AI-03 không cần biết toàn bộ quá trình retrieval tạo package đó như thế nào.

Model-specific transformation xảy ra sau contract:

GenerationContextV1

↓

Adapter-specific Prompt / Condition Builder

↓

Model-specific Input

↓

StoryDiffusion / Proposed Method (SAMIM) / External API

Đây là separation of concerns quan trọng.

# **33. Context Package Requirements**

**CTX-PKG-001  
**Context Package SHALL identify Project and Branch.

**CTX-PKG-002  
**All Character states included in the package SHALL correspond to the target timeline position.

**CTX-PKG-003  
**Draft information MUST be distinguishable from Canon information.

**CTX-PKG-004  
**Historical Facts SHOULD include provenance.

**CTX-PKG-005  
**The package SHALL contain only information authorized for the current Project.

# **34. Provenance**

Provenance cho biết một Fact đến từ đâu.

Ví dụ:

Fact

→ Aria no longer owns Silver Sword.

Source

→ Chapter 12

→ Scene 03

→ Event EV-193

Provenance cần thiết cho:

- debugging;

- consistency explanation;

- impact analysis;

- user inspection;

- research evaluation.

# **35. Narrative Consistency Architecture**

Consistency không nên chỉ kiểm tra sau khi text đã được generate.

Nên có ba giai đoạn.

PRE-GENERATION

→ Đưa constraint đúng vào Context Package.

GENERATION

→ Model được yêu cầu tuân thủ Canon.

POST-GENERATION

→ Kiểm tra output mới với Story Core.

PRE-APPROVAL

→ Nếu có conflict nghiêm trọng, cảnh báo user.

Consistency tốt nhất đến từ cả **prevention** và **detection**.

# **36. Consistency Categories**

V1 cần kiểm tra ít nhất các nhóm sau.

| **Category**   | **Ví dụ contradiction**                            |
|----------------|----------------------------------------------------|
| Identity       | Tuổi/tên/background thay đổi vô lý                 |
| Physical State | Nhân vật đã bị thương nhưng hành động như không bị |
| Possession     | Dùng item đã mất                                   |
| Location       | Nhân vật xuất hiện ở nơi không thể tới             |
| Status         | Nhân vật chết nhưng xuất hiện bình thường          |
| Knowledge      | Biết bí mật chưa từng biết                         |
| Relationship   | Hành vi trái trạng thái quan hệ mà không có lý do  |
| Timeline       | Event B xảy ra trước điều kiện Event A             |
| World Rule     | Magic xuất hiện trong thế giới cấm magic           |
| Visual State   | Outfit/injury/image sai trạng thái hiện tại        |

# **37. Consistency Check Pipeline**

Generated Draft

→ Output mới từ AI.

↓

Claim Extraction

→ Trích các assertion quan trọng.

↓

Relevant Canon Retrieval

→ Lấy Fact/State/Event liên quan.

↓

Comparison

→ So sánh assertion với Canon.

↓

Classification

→ Valid / Possible Conflict / Contradiction.

↓

Severity Assignment

→ Info / Warning / Critical.

↓

Consistency Report

→ Trả kết quả cho user và backend.

# **38. Consistency Severity**

### **INFO**

Không phải lỗi, chỉ đáng chú ý.

Ví dụ:

> Character sử dụng một outfit chưa từng được mô tả.

### **WARNING**

Có khả năng inconsistent nhưng vẫn có thể hợp lý.

Ví dụ:

> Aria tỏ ra thân thiện với Kael dù Relationship hiện tại là Distrust.

Có thể đây là sự giả vờ hoặc character development.

### **CRITICAL**

Mâu thuẫn trực tiếp với Canon.

Ví dụ:

Canon

→ Kael died in Chapter 20.

Draft

→ Kael enters the café normally in Chapter 30.

# **39. Human Authority over Consistency**

Consistency Engine không được tự động coi mọi contradiction là lỗi tuyệt đối.

Storytelling có thể chứa:

- plot twist;

- unreliable narrator;

- deception;

- dream;

- hallucination;

- flashback;

- resurrection;

- deliberate contradiction.

Do đó user phải có quyền:

Accept Warning

Ignore

Explain / Add Rule

Modify Draft

Update Canon

AI đóng vai trò **continuity assistant**, không phải người quyết định nội dung cuối cùng.

# **40. Consistency Report**

Một report nên trả về:

Issue ID

→ CONS-392

Severity

→ Critical

Category

→ Possession

Current Draft

→ "Aria draws the Silver Sword."

Canon Evidence

→ Sword destroyed in Chapter 12 / Scene 03.

Explanation

→ No subsequent Event restores or replaces the item.

Suggested Actions

→ Rewrite

→ Add recovery event

→ Ignore intentionally

Report phải giải thích **tại sao** issue được phát hiện.

Không nên chỉ nói:

> “Inconsistent.”

# **41. Consistency Requirements**

**CONS-001  
**Consistency findings SHALL include evidence whenever evidence is available.

**CONS-002  
**The system SHALL distinguish Warning from Critical contradiction.

**CONS-003  
**Consistency warnings SHALL NOT automatically modify user content.

**CONS-004  
**Users SHALL be able to intentionally ignore a warning.

**CONS-005  
**Ignored warnings SHOULD retain audit information.

# **42. Impact Analysis**

Consistency kiểm tra nội dung mới.

Impact Analysis giải quyết chiều ngược lại:

> Điều gì xảy ra khi user thay đổi Canon cũ?

Ví dụ user thay Chapter 20:

Old Canon

→ Kael betrays Aria.

New Canon

→ Kael secretly protects Aria.

Các Chapter sau có thể đang dựa trên betrayal cũ.

# **43. Dependency Model**

Canonical entity nên tạo dependency có thể truy vết.

Event EV-20

"Kael betrays Aria"

│

├── Relationship State

│ → Aria distrusts Kael

│

├── Canon Fact

│ → Kael betrayed Aria

│

├── Chapter 21

│ → Aria leaves group

│

└── Chapter 24

→ confrontation scene

Khi EV-20 thay đổi, hệ thống có thể xác định downstream dependencies.

# **44. Impact Classification**

Affected content được phân thành:

| **Status**   | **Meaning**                         |
|--------------|-------------------------------------|
| Valid        | Không bị ảnh hưởng                  |
| Needs Review | Có dependency nhưng chưa chắc sai   |
| Invalidated  | Trực tiếp dựa vào Canon đã thay đổi |

# **45. Canon Change Workflow**

User edits approved content

→ Canon Change Candidate

↓

Detect Changed Facts / Events

↓

Dependency Traversal

↓

Impact Report

↓

User Decision

├── Keep downstream content

├── Repair selected content

├── Regenerate affected content

└── Create new Branch

Không được tự động rewrite hàng chục Chapter phía sau mà không có sự đồng ý.

# **46. Example — Generate Chapter 100**

Giả sử user đang tạo Scene trong Chapter 100.

Request:

> “Aria gặp Kael lần đầu sau nhiều năm và hỏi về chiếc nhẫn của mẹ.”

Toàn bộ Story có 99 Chapter trước đó.

Hệ thống không gửi tất cả chúng.

Context Engine có thể tạo:

User Instruction

→ Aria meets Kael and asks about mother's ring.

Story Rules

→ Main rules relevant to scene.

Aria Current State

→ Current location, goal, knowledge.

Kael Current State

→ Current status and possessions.

Relationship

→ They have not met since Chapter 32.

Relevant Event

→ Chapter 8: mother gives ring to Kael.

Relevant Fact

→ Kael still possesses ring.

Character Knowledge

→ Aria knows mother owned ring.

→ Aria does NOT know mother gave it to Kael.

Recent Context

→ Chapter 99 summary.

Current Arc

→ Arc summary.

Model lúc này có đủ thông tin để tạo Scene hợp lý mà không cần đọc lại hàng trăm nghìn từ.

# **47. Example — Knowledge Violation**

AI tạo:

> Aria: “My mother gave you that ring before she died.”

Nhưng Memory cho biết:

Story Truth

→ Mother gave Kael the ring.

Aria Knowledge

→ Aria has never learned this.

Consistency Engine tạo:

Severity

→ Critical

Category

→ Character Knowledge

Issue

→ Aria refers to information not present in her knowledge state.

Evidence

→ Ring transfer occurred in Chapter 8 while Aria was absent.

Developer có thể test requirement này mà không cần hiểu toàn bộ thuật toán NLP phía sau.

# **48. Data Freshness**

Sau mỗi Canon update, affected memory phải được đánh dấu cần cập nhật.

Không được để:

Story Core = new truth

Memory Index = old truth

trong thời gian dài.

Memory pipeline cần hỗ trợ state:

CURRENT

STALE

REBUILDING

INVALIDATED

# **49. Retrieval Consistency**

**CTX-RET-001  
**Only Canon and permitted Reference data SHALL be used as authoritative retrieval sources.

**CTX-RET-002  
**Draft content SHALL NOT be presented to the model as Canon.

**CTX-RET-003  
**Inactive historical state SHALL NOT be treated as Current State.

**CTX-RET-004  
**Retrieval SHALL respect Branch boundaries.

**CTX-RET-005  
**Retrieval SHALL respect temporal validity.

**CTX-RET-006  
**Semantic similarity alone SHALL NOT determine Canon authority.

# **50. Memory Requirements**

**MEM-001  
**Memory SHALL be reconstructable from Canon data.

**MEM-002  
**Deleting derived Memory MUST NOT delete Canon source.

**MEM-003  
**Every automatically generated Canon Fact SHOULD preserve provenance.

**MEM-004  
**Memory extraction failures SHALL NOT invalidate approved story content.

**MEM-005  
**Memory MUST support long-range retrieval without requiring full-manuscript context.

# **51. Performance Considerations**

Context Retrieval nằm trên đường critical path của generation.

Hệ thống cần tránh:

Generation Request

→ scan toàn bộ database

→ embed toàn bộ story lại

→ gọi model

Embedding SHOULD được tính khi memory được tạo/cập nhật.

Retrieval tại runtime chủ yếu nên:

filter

→ search

→ rank

→ assemble

# **52. Observability**

Một generation request cần có thể giải thích context nào đã được sử dụng.

Context trace tối thiểu:

ContextRequestId

TaskType

BranchId

TargetScene

RetrievedFactIds

RetrievedEventIds

CharacterStateVersions

RelationshipStateVersions

RetrievalStrategy

FinalContextSize

Không nhất thiết phải hiển thị toàn bộ thông tin kỹ thuật này cho end user, nhưng developer/researcher cần truy cập để debug và đánh giá.

# **53. Research Relevance**

AI-02 cũng tạo nền tảng cho experimental evaluation.

Các hướng đánh giá có thể so sánh:

Baseline

→ Recent context only.

vs.

Memory Retrieval

→ Recent + semantic memory.

vs.

Structured Memory

→ State + event + fact + relationship.

vs.

Proposed Full System

→ Structured + semantic + temporal + knowledge constraints.

Các metric cụ thể thuộc research/evaluation document, nhưng architecture phải cho phép thay đổi strategy để tiến hành thí nghiệm.

# **54. Functional Requirements Summary**

| **ID**   | **Requirement**                                                                |
|----------|--------------------------------------------------------------------------------|
| CTX-001  | System SHALL build task-specific Context Packages                              |
| CTX-002  | Mandatory Canon State SHALL be loaded independently of semantic search         |
| CTX-003  | Historical information MAY be retrieved semantically                           |
| CTX-004  | Retrieval SHALL respect Branch and temporal scope                              |
| CTX-005  | Context SHALL distinguish Canon, Reference and Draft                           |
| MEM-001  | Memory SHALL originate from approved Canon                                     |
| MEM-002  | State transition SHALL retain provenance                                       |
| KNOW-001 | Character Knowledge SHALL remain distinct from global Story Truth              |
| CONS-001 | Generated content SHALL be checkable against relevant Canon                    |
| CONS-002 | Consistency findings SHALL include severity                                    |
| IMP-001  | Editing historical Canon SHALL trigger impact analysis when dependencies exist |
| IMP-002  | Downstream content SHALL NOT be silently rewritten                             |

# **55. Non-Functional Requirements**

### **Explainability**

Consistency issue và retrieved context phải có khả năng truy nguyên.

### **Maintainability**

Retrieval/ranking implementation phải có thể thay đổi mà không sửa Story Core.

### **Reproducibility**

Research experiment phải có khả năng biết strategy và context nào đã được dùng.

### **Scalability**

Context construction không được yêu cầu tải toàn bộ manuscript cho mỗi generation.

### **Reliability**

Failure của vector search không được làm mất Canon.

### **Isolation**

Memory của Project/Branch khác không được rò vào context hiện tại.

# **56. Acceptance Criteria**

### **AC-CTX-01 — Long-range retrieval**

**Given** một Fact quan trọng được thiết lập ở Chapter 5,  
**and** Fact vẫn còn hiệu lực ở Chapter 80,  
**when** Chapter 80 tạo Scene liên quan tới Fact đó,  
**then** Context Engine phải có khả năng đưa Fact đó vào Context Package mà không cần gửi toàn bộ Chapter 5–79.

### **AC-CTX-02 — Expired state**

**Given** Aria sở hữu Sword từ Chapter 3 đến Chapter 12,  
**and** Sword bị phá hủy ở Chapter 12,  
**when** Scene Chapter 30 được tạo,  
**then** Current Context không được xem Sword là possession hiện tại.

### **AC-CTX-03 — Branch isolation**

**Given** Kael chết trong Alternative Branch nhưng sống trong Main Branch,  
**when** user generation Main Branch,  
**then** Event “Kael died” từ Alternative Branch không được sử dụng làm Canon Context.

### **AC-MEM-01 — Draft isolation**

**Given** AI tạo một Draft nói Aria bị thương,  
**when** Draft chưa được Approve,  
**then** Character Current State không được cập nhật thành injured.

### **AC-KNOW-01 — Character knowledge**

**Given** một Canon Fact chưa từng được tiết lộ cho Aria,  
**when** AI tạo dialogue cho Aria,  
**then** Consistency Engine phải có khả năng phát hiện nếu Aria sử dụng Fact đó như kiến thức đã biết.

### **AC-CONS-01 — Evidence**

**Given** generated content mâu thuẫn với Canon,  
**when** hệ thống tạo consistency warning,  
**then** warning phải tham chiếu được Canon evidence gây ra conflict.

### **AC-IMP-01 — Historical edit**

**Given** user thay đổi một Canon Event cũ,  
**and** nhiều later scenes phụ thuộc vào Event đó,  
**when** thay đổi được xác nhận,  
**then** hệ thống phải đánh dấu nội dung downstream cần review thay vì tự động rewrite chúng.

# **57. Risks and Mitigations**

| **Risk**                                             | **Consequence**                  | **Mitigation**                     |
|------------------------------------------------------|----------------------------------|------------------------------------|
| Semantic retrieval lấy thông tin gần nghĩa nhưng sai | Hallucination/context pollution  | Structured filter + provenance     |
| Summary mất chi tiết                                 | AI bỏ sót Fact                   | Kết hợp summary và structured Fact |
| Memory extraction sai                                | State bị sai                     | Validation + Canon source          |
| Context quá lớn                                      | Tăng latency/cost                | Ranking + budget + compression     |
| Context quá nhỏ                                      | Bỏ sót event quan trọng          | Hybrid retrieval                   |
| Old state được lấy nhầm                              | Continuity error                 | Temporal validity                  |
| Cross-branch leakage                                 | Story contradiction nghiêm trọng | Branch filtering                   |
| Character knows too much                             | Narrative inconsistency          | Knowledge model                    |
| User edit Canon cũ                                   | Downstream story invalid         | Impact analysis                    |

# **58. Implementation Guidance**

Từ góc nhìn developer, AI-02 có thể được chia thành các module logic:

ContextEngine

→ Điều phối quá trình xây context.

StructuredRetriever

→ Lấy state/rule/entity relation chính xác.

SemanticRetriever

→ Tìm memory bằng pgvector.

ContextRanker

→ Rank candidates.

ContextAssembler

→ Xây Context Package.

MemoryExtractor

→ Phân tích approved content.

MemoryIndexer

→ Tạo/search embedding.

ConsistencyChecker

→ Kiểm tra Draft.

ImpactAnalyzer

→ Phân tích dependency khi Canon thay đổi.

Đây là **logical component boundary**, không bắt buộc mỗi component phải trở thành microservice.

Trong V1, chúng hoàn toàn có thể cùng nằm trong Python AI Service hoặc được chia giữa ASP.NET Backend và AI Service tùy responsibility.

# **59. Responsibility Boundary for Developers**

Một ranh giới quan trọng cần giữ:

| **Concern**                   | **Backend** | **AI Service** |
|-------------------------------|-------------|----------------|
| Canon authority               | ✓           |                |
| Branch validation             | ✓           |                |
| Current State persistence     | ✓           |                |
| Structured relationship query | ✓/shared    |                |
| Semantic retrieval            |             | ✓              |
| Context ranking               |             | ✓              |
| Prompt context assembly       |             | ✓              |
| Memory extraction proposal    |             | ✓              |
| Validate entity existence     | ✓           |                |
| Commit memory/state           | ✓           |                |
| Consistency reasoning         |             | ✓              |
| User approval                 | ✓           |                |

AI Service **phát hiện và đề xuất**.

Backend **xác thực và ghi nhận sự thật**.

# **60. Final System Behavior**

Nếu toàn bộ specification này được hiện thực đúng, hệ thống sẽ có hành vi như sau:

Story càng dài

→ lượng Canon ngày càng lớn.

Nhưng mỗi generation

→ chỉ lấy context liên quan.

Character thay đổi

→ Current State được cập nhật từ Canon.

Thông tin cũ quan trọng

→ vẫn có thể được retrieval.

Character chưa biết Fact

→ Fact không được coi là Character Knowledge.

AI tạo contradiction

→ Consistency Engine phát hiện và giải thích.

User sửa Canon quá khứ

→ Impact Analyzer tìm nội dung bị ảnh hưởng.

User vẫn luôn

→ là người quyết định Canon cuối cùng.

Đây chính là cơ chế giúp sản phẩm đi từ một ứng dụng **“gửi prompt cho AI để viết truyện”** thành một **long-range narrative system có state, memory, provenance và consistency control**.
