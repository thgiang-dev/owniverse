# **RESEARCH-03 — Scene-Aware Multi-Character Identity Memory Method Specification**

**Project:** Character-Consistent Long-Range Story Generation  
**Document ID:** RESEARCH-03  
**Document Type:** Proposed Research Method Specification  
**Method Name:** Scene-Aware Multi-Character Identity Memory  
**Abbreviation:** SAMIM  
**Version:** 0.1  
**Status:** Provisional — Pending Baseline Evidence  
**Parent Documents:** RESEARCH-01, RESEARCH-02  
**Related Document:** RESEARCH-04  
**Research Area:** Story Visualization / Character-Consistent Image Generation

# **1. Purpose**

Tài liệu này đặc tả phương pháp nghiên cứu dự kiến cho bài toán:

> **Character-Consistent Long-Range Story Generation**

Phương pháp hướng tới ba vấn đề chính:

1.  duy trì identity của recurring character khi character biến mất trong nhiều scene rồi xuất hiện trở lại;

2.  duy trì nhiều character identity riêng biệt trong cùng một story;

3.  cho phép scene-specific appearance thay đổi mà không làm mất core identity.

Hướng nghiên cứu hiện tại đã xác định mỗi character có một identity memory riêng và tại mỗi scene chỉ memory của những character thực sự xuất hiện mới được sử dụng.

Tuy nhiên, tài liệu nguồn cũng xác định rõ rằng cấu trúc memory, retrieval mechanism và cơ chế tích hợp vào diffusion model phải được quyết định sau khi baseline được phân tích.

Do đó RESEARCH-03 v0.1 định nghĩa:

- kiến trúc logic bắt buộc;

- interface giữa các thành phần;

- các design candidate;

- decision points;

- yêu cầu thực nghiệm;

nhưng chưa khóa implementation model-specific nếu chưa có bằng chứng từ RESEARCH-02.

# **2. Method Status**

Trạng thái hiện tại:

> **Research Method Candidate**

Không được hiểu là:

> Final Proposed Method.

Phương pháp chỉ được nâng lên trạng thái:

PROVISIONAL

→ BASELINE-INFORMED

→ IMPLEMENTED

→ EXPERIMENTALLY-VALIDATED

sau khi từng bước có bằng chứng tương ứng.

# **3. Source-Grounded Design Constraints**

Bốn nguyên tắc sau được xem là nền tảng đã xác định.

## **C1 — Per-Character Identity Memory**

Với tập character:

C={c1,c2,…,cN}C=\\c_1,c_2,\ldots,c_N\\

duy trì tập identity memory:

M={M1,M2,…,MN}M=\\M_1,M_2,\ldots,M_N\\

trong đó:

MiM_i

chỉ đại diện cho identity information của:

cic_i

## **C2 — Scene-Aware Selection**

Tại scene:

sts_t

chỉ một subset:

Ct⊆CC_t\subseteq C

xuất hiện.

Do đó chỉ memory:

Mt={Mi∣ci∈Ct}M_t=\\M_i\mid c_i\in C_t\\

được đưa vào generation.

Không mặc định đưa identity information của toàn bộ story vào cùng một lần generation.

## **C3 — Long-Range Retrieval**

Khi character tái xuất hiện sau nhiều scene:

ci@sa→absence→ci@sbc_i@s_a \rightarrow absence \rightarrow c_i@s_b

hệ thống phải có khả năng sử dụng lại identity representation của chính cic_i thay vì chỉ phụ thuộc vào nearby scenes.

## **C4 — Identity / Appearance Separation**

Identity information phải được phân biệt với scene-dependent attributes.

Ví dụ nguồn hiện tại phân biệt:

**Stable Identity**

- eye characteristics;

- core hair characteristics;

- scar;

- facial/physical identity;

với:

**Scene Appearance**

- outfit;

- wet hair;

- expression;

- injury/state.

Application design cũng xác định visual reference dùng để giúp model hiểu “character này chính xác là ai”, còn Visual State mô tả character đang trông như thế nào tại thời điểm hiện tại.

# **4. Research Goal**

Mục tiêu của SAMIM không phải tối đa hóa similarity giữa tất cả ảnh của cùng một character.

Mục tiêu là tối ưu đồng thời:

Identity PreservationIdentity\\ Preservation ++ Identity SeparationIdentity\\ Separation ++ Scene ComplianceScene\\ Compliance

Nói cách khác:

> Same character phải recognizable.
>
> Different characters phải distinguishable.
>
> Scene-specific appearance vẫn phải thay đổi đúng.

# **5. Method Motivation**

RESEARCH-02 sẽ xác định failure thực tế.

Nếu baseline cho thấy:

### **Failure A — Long-range identity loss**

thì cần cơ chế không phụ thuộc hoàn toàn vào recent image context.

### **Failure B — Identity leakage**

thì cần giới hạn interaction giữa identity representations.

### **Failure C — Appearance over-constraint**

thì identity và mutable appearance cần được condition riêng.

### **Failure D — Secondary character degradation**

thì mỗi recurring character cần có representation độc lập thay vì một global story representation duy nhất.

Mỗi component của SAMIM phải trace được tới ít nhất một failure đã xác nhận.

# **6. High-Level Method**

Luồng logic dự kiến:

Story / Scene

↓

Active Character Resolution

↓

Character-Specific Identity Retrieval

↓

Scene-Specific Appearance Resolution

↓

Identity / Appearance Conditioning Assembly

↓

Multi-Character Isolation

↓

Image Generation

↓

Identity + Scene Validation

↓

Generated Candidate

Đây là **logical research architecture**.

Nó chưa chỉ định attention layer, encoder hay diffusion implementation cụ thể.

# **7. Core Notation**

Story:

S={s1,s2,…,sT}S=\\s_1,s_2,\ldots,s_T\\

Character set:

C={c1,c2,…,cN}C=\\c_1,c_2,\ldots,c_N\\

Active characters tại scene sts_t:

Ct⊆CC_t\subseteq C

Identity memory của character cic_i:

MiM_i

Scene-dependent appearance của character cic_i tại scene sts_t:

Ai,tA\_{i,t}

Scene description:

DtD_t

Style/context:

PtP_t

Generated image:

ItI_t

# **8. Conceptual Generation Function**

Generation được mô hình hóa ở mức khái niệm:

It=G(Dt,Pt,{Mi}ci∈Ct,{Ai,t}ci∈Ct)I_t = G( D_t, P_t, \\M_i\\\_{c_i\in C_t}, \\A\_{i,t}\\\_{c_i\in C_t} )

Trong đó:

- DtD_t: scene content;

- PtP_t: visual/style context;

- MiM_i: identity information;

- Ai,tA\_{i,t}: mutable scene appearance.

Điểm quan trọng:

Mi≠Ai,tM_i \neq A\_{i,t}

# **9. Identity Memory Definition**

## **9.1 Logical Definition**

Identity Memory MiM_i là representation giúp hệ thống trả lời:

> **“Character cic_i là ai về mặt thị giác?”**

Nó không nên được xem là toàn bộ character profile.

Identity Memory không mặc định chứa:

- current emotion;

- current location;

- current outfit;

- temporary injury;

- current pose.

Những dữ liệu đó thuộc scene/state context.

# **10. What May Belong to Identity Memory**

Ở mức conceptual, MiM_i có thể đại diện cho:

- facial identity;

- distinctive physical traits;

- persistent hair characteristics;

- identity-defining marks;

- stable body characteristics;

- canonical visual references.

Nguồn domain hiện tại cho phép research model sử dụng:

- character identity;

- previous appearance;

- scene position;

- reference images;

nhưng cố ý không định nghĩa embedding, memory vector, attention mechanism hoặc model architecture.

# **11. Identity Memory Representation**

## **DECISION PENDING — RESEARCH-02 / Prototype Evaluation**

RESEARCH-03 v0.1 không khóa MiM_i là một representation duy nhất.

Các design candidate có thể bao gồm:

### **Candidate A — Reference Image Memory**

Mi={Ri1,Ri2,...,Rik}M_i=\\R\_{i1},R\_{i2},...,R\_{ik}\\

Memory là một tập canonical visual references.

### **Candidate B — Feature Memory**

Mi={fi1,fi2,...,fik}M_i=\\f\_{i1},f\_{i2},...,f\_{ik}\\

Memory lưu visual feature representations được trích từ references.

### **Candidate C — Aggregated Identity Representation**

Mi=fˉiM_i=\bar f_i

một representation tổng hợp từ nhiều reference.

### **Candidate D — Hybrid Memory**

Mi=(Ri,Fi,μi)M_i=(R_i,F_i,\mu_i)

kết hợp reference assets và derived representations.

Việc chọn candidate nào phải dựa trên prototype + RESEARCH-02 evidence.

# **12. Memory Source Policy**

Nguồn ưu tiên ban đầu của Identity Memory phải là:

> **Canonical Character References**

Application hiện tại quy định user có thể chọn visual reference làm canonical reference và regeneration không được tự động thay thế reference đó.

Do đó SAMIM không được mặc định:

> generated image mới = identity truth mới.

# **13. Memory Initialization**

Với character mới:

Mi(0)M_i^{(0)}

được tạo từ canonical character reference.

Logical flow:

Canonical Reference

↓

Identity Representation Extraction

↓

Character-Specific Memory

↓

M_i

Exact extraction function:

Eid(⋅)E\_{id}(\cdot)

là model-specific và chưa khóa.

# **14. Memory Write Policy**

## **DECISION PENDING**

Có hai strategy chính cần thử nghiệm.

### **Strategy 1 — Static Identity Memory**

Memory chỉ đến từ canonical reference.

Mit=Mi0M_i^{t}=M_i^{0}

qua toàn bộ story.

Ưu điểm:

- không tích lũy generation error;

- identity source rõ ràng.

Rủi ro:

- reference có thể không đủ cho nhiều pose/view.

### **Strategy 2 — Controlled Dynamic Memory**

Một generated appearance có thể bổ sung memory nếu vượt qua validation.

Mit+1=Update(Mit,It)M_i^{t+1} = Update(M_i^t,I_t)

nhưng chỉ khi:

Accept(It)=TrueAccept(I_t)=True

và:

IdentityQuality(It)≥τIdentityQuality(I_t)\geq \tau

Threshold và update rule chưa được khóa.

# **15. No Unvalidated Memory Write**

Bất kể strategy cuối cùng:

> output lỗi không được tự động cập nhật Identity Memory.

Nếu generation bị identity drift rồi được dùng làm reference cho lần sau:

errort→memory→errort+1error_t \rightarrow memory \rightarrow error\_{t+1}

có thể tạo error propagation.

Do đó memory update phải có validation boundary.

# **16. Scene-Aware Character Resolver**

Input:

sts_t

Output:

CtC_t

Resolver xác định character nào thực sự xuất hiện trong scene.

Ví dụ:

Story có:

C={A,B,C,D,E}C=\\A,B,C,D,E\\

Scene 12 chỉ có:

C12={A,C}C\_{12}=\\A,C\\

thì generation không cần identity memory của:

B,D,EB,D,E

# **17. Scene-Aware Memory Retrieval**

Retrieval function:

RM(Ct,M)R_M(C_t,M)

trả về:

Mt={Mi∣ci∈Ct}M_t = \\M_i\mid c_i\in C_t\\

Mục tiêu:

> Identity context phải phụ thuộc scene participation.

Không phải semantic similarity quyết định một character có được đưa vào hay không nếu hệ thống đã biết active character list.

# **18. Retrieval Invariant**

Nếu:

cj∉Ctc_j\notin C_t

thì mặc định:

Mj∉MtM_j\notin M_t

trừ khi một experimental condition có chủ đích kiểm tra global memory.

Điều này tạo trực tiếp ablation:

### **Global Memory**

Mt=MM_t=M

so với:

### **Scene-Aware Memory**

Mt={Mi∣ci∈Ct}M_t=\\M_i\mid c_i\in C_t\\

# **19. Long-Range Retrieval Principle**

Identity retrieval không được phụ thuộc vào:

> character phải xuất hiện ở scene ngay trước.

Ví dụ:

Scene 01 → A

Scene 02 → B

Scene 03 → B

Scene 04 → C

...

Scene 10 → A

Scene 10 phải retrieve:

MAM_A

trực tiếp.

Không cần propagate identity A qua Scene 02–09.

Đây là mục tiêu long-range cốt lõi của phương pháp.

# **20. Identity / Appearance Decomposition**

Với character:

cic_i

tại scene:

sts_t

generation context được chia:

CharacterContexti,t=(Mi,Ai,t)CharacterContext\_{i,t} = (M_i,A\_{i,t})

Trong đó:

### **MiM_i**

“Who is this character?”

### **Ai,tA\_{i,t}**

“How does this character appear in this scene?”

# **21. Appearance State**

Ai,tA\_{i,t} có thể chứa:

- outfit;

- expression;

- pose;

- temporary hairstyle condition;

- injury;

- equipment;

- weather effect;

- current physical state.

Nguồn hiện tại cũng thiết kế image generation ưu tiên current visual state, pose, camera, location, style và character reference.

# **22. Example**

Character:

Aria

Identity:

green eyes

black hair

facial structure

scar on left cheek

Scene 05:

school uniform

happy

clean hair

Scene 30:

black winter coat

left arm bandaged

wet hair

angry

Hệ thống cần:

Identity(Scene05)≈Identity(Scene30)Identity(Scene05) \approx Identity(Scene30)

nhưng:

Appearance(Scene05)≠Appearance(Scene30)Appearance(Scene05) \neq Appearance(Scene30)

# **23. Appearance Override Rule**

Nếu scene state yêu cầu thay đổi mutable attribute:

Default outfit = white shirt

Scene outfit = winter coat

thì:

SceneAppearance\>DefaultAppearanceSceneAppearance \> DefaultAppearance

đối với mutable attribute đó.

Nhưng scene instruction không nên tự động override identity-defining attribute.

# **24. Identity Evolution**

Một long story có thể có canonical identity-changing event.

Ví dụ:

> character mất một mắt.

Hoặc:

> character có permanent scar mới.

Vì vậy “stable identity” không có nghĩa:

> immutable forever.

Mà nên hiểu:

> Identity-defining attributes remain stable unless an explicit canonical event changes them.

Domain specification hiện tại cũng đã xác định stable identity có thể có temporal evolution trong trường hợp story event thay đổi canonical identity.

Do đó MiM_i có thể cần version theo story time nếu research implementation sử dụng evolving identity.

# **25. Temporal Identity Versioning**

Conceptually:

Mi(v)M_i^{(v)}

trong đó vv đại diện canonical identity version.

Generation Scene 5 phải dùng:

Miv1M_i^{v_1}

nếu identity-changing event chỉ xảy ra Scene 20.

Không được dùng future identity cho past scene regeneration.

# **26. Multi-Character Conditioning**

Với:

Ct={ca,cb}C_t=\\c_a,c_b\\

generation nhận:

Mt={Ma,Mb}M_t=\\M_a,M_b\\

và:

At={Aa,t,Ab,t}A_t=\\A\_{a,t},A\_{b,t}\\

Mục tiêu:

Identity(It,a)≈MaIdentity(I_t,a)\approx M_a Identity(It,b)≈MbIdentity(I_t,b)\approx M_b

đồng thời:

Leakage(a→b)→lowLeakage(a\rightarrow b)\rightarrow low Leakage(b→a)→lowLeakage(b\rightarrow a)\rightarrow low

# **27. Character Binding Problem**

Một trong những vấn đề cần giải quyết là:

> Làm sao model biết identity information nào thuộc character nào trong output?

Logical requirement:

Character A

↔ Memory A

↔ Appearance A

↔ Scene role A

Character B

↔ Memory B

↔ Appearance B

↔ Scene role B

Không được flatten thành:

Memory A + Memory B + Appearance A + Appearance B

→ one undifferentiated condition

nếu điều đó gây identity mixing.

# **28. Identity Isolation Layer**

SAMIM cần một logical component:

> **Identity Isolation Layer**

Responsibility:

1.  giữ association giữa Character ID và identity representation;

2.  hạn chế cross-character contamination;

3.  cung cấp condition riêng biệt cho từng character.

Exact implementation:

> **DECISION PENDING**

# **29. Isolation Candidate Designs**

Các candidate có thể được thử nghiệm.

### **Candidate I — Independent Identity Tokens**

Mỗi character có identity token/representation riêng.

### **Candidate II — Spatially Bound Conditioning**

Identity representation được ràng buộc với spatial region của character.

### **Candidate III — Mask-Guided Character Conditioning**

Mỗi character sử dụng mask riêng trong relevant attention/control operation.

### **Candidate IV — Character-Specific Attention Routing**

Condition của character được route tới subset tương ứng trong generation.

Các candidate trên là **hướng thiết kế cần kiểm chứng**, không phải claim rằng tài liệu nguồn đã chọn một trong số chúng.

# **30. Why Isolation Exists**

Identity isolation chỉ nên được giữ trong final method nếu RESEARCH-02 cho thấy:

- leakage;

- feature mixing;

- character confusion;

- hoặc secondary identity degradation

là failure có ý nghĩa.

Nếu baseline không có failure này đáng kể:

> component isolation có thể bị loại khỏi final method.

# **31. Conditioning Assembly**

Trước generation, system tạo:

Zt=Assemble(Dt,Pt,Mt,At)Z_t = Assemble( D_t, P_t, M_t, A_t )

Trong đó ZtZ_t là logical generation condition.

Nó phải giữ được mapping:

ci→(Mi,Ai,t)c_i\rightarrow(M_i,A\_{i,t})

không chỉ nối toàn bộ context thành một unordered block.

# **32. Model Integration Boundary**

Nguồn hiện tại đã xác định research model phải nằm sau một abstraction để application có thể dùng StoryDiffusion hoặc ProposedMethod mà không thay product workflow.

Do đó application-level interface có thể được hiểu:

GenerateSceneImage(request)

trong khi research adapter tự xử lý SAMIM.

Application không cần biết:

- embedding type;

- attention layer;

- memory vector;

- diffusion modification.

# **33. Research Model Interface**

Logical input:

SceneGenerationInput

scene_description

active_characters

for each character:

character_id

canonical_reference

identity_version

scene_appearance

style_context

generation_parameters

Logical output:

GeneratedCandidate

image

method_version

identity_memory_versions

generation_metadata

diagnostics

# **34. Identity Memory Manager**

Logical responsibility:

InitializeMemory(character)

LoadMemory(character, version)

UpdateMemory(character, candidate)

ValidateMemoryUpdate(...)

Không nhất thiết là một software service riêng.

Đây là research component boundary.

# **35. Active Character Selector**

Responsibility:

ResolveActiveCharacters(scene)

Output:

\[C1, C4, C7\]

Không cần vector search nếu scene-character relation đã được biết chính xác.

# **36. Appearance Resolver**

Responsibility:

ResolveAppearance(character, sceneTime)

Output ví dụ:

outfit = black coat

injury = left arm bandaged

expression = angry

hair_state = wet

Không overwrite identity memory.

# **37. Identity Retriever**

Responsibility:

RetrieveIdentity(character, sceneTime)

Output:

MivM_i^{v}

đúng identity version tại thời điểm scene.

# **38. Conditioning Adapter**

Responsibility:

> Chuyển logical identity/appearance context sang representation model cụ thể hiểu được.

Ví dụ:

SAMIM logical memory

↓

StoryDiffusion-specific adapter

hoặc:

SAMIM logical memory

↓

custom diffusion integration

Do đó research architecture không bị khóa quá sớm vào một implementation.

# **39. Integration with Diffusion**

## **DECISION PENDING**

Nguồn ban đầu ghi rõ cơ chế tích hợp memory vào diffusion chưa được xác định.

Các integration location cần khảo sát có thể gồm:

- conditioning input;

- cross-attention;

- self-attention;

- reference attention;

- feature injection;

- adapter/control module.

Final choice phải trả lời:

> Failure nào đang được giải quyết?

và:

> Ablation có chứng minh integration đó cần thiết hay không?

# **40. Reference Usage**

Canonical reference được coi là strong identity source.

Visual State là scene source.

Do đó generation logic phải tránh việc prompt text trở thành nguồn identity duy nhất.

Application design hiện tại cũng đã định nghĩa:

> Character Reference giúp model biết “Aria chính xác là người nào”, thay vì chỉ biết “một cô gái tóc đen”.

# **41. Previous Appearance Usage**

Research model có thể sử dụng previous appearance.

Tuy nhiên cần phân biệt:

### **Reference Identity**

Canonical source.

### **Previous Generated Appearance**

Historical evidence.

Previous generation không mặc định đáng tin bằng canonical reference.

# **42. Previous Appearance Candidate Policy**

Có thể test:

### **P0**

Không dùng previous generated image.

### **P1**

Dùng previous appearance gần nhất.

### **P2**

Dùng selected previous appearances.

### **P3**

Dùng aggregated historical identity representation.

Đây có thể trở thành ablation trong RESEARCH-04.

# **43. Error Propagation Risk**

Nếu dùng previous output liên tục:

I1→I2→I3→I4I_1\rightarrow I_2\rightarrow I_3\rightarrow I_4

một identity error tại:

I2I_2

có thể truyền:

I3,I4I_3,I_4

SAMIM hướng tới khả năng:

Canonical Identity Memory→ItCanonical\\ Identity\\ Memory \rightarrow I_t

để giảm phụ thuộc tuyệt đối vào chained history.

Đây là một research motivation cần được RESEARCH-02 xác nhận.

# **44. Character-Specific Memory Independence**

Ideal logical property:

Update(Mi)Update(M_i)

không trực tiếp thay đổi:

MjM_j

với:

i≠ji\neq j

Điều này hỗ trợ identity separation.

# **45. Memory Capacity**

## **DECISION PENDING**

Nếu memory chứa nhiều references/features:

∣Mi∣\|M_i\|

không nên tăng vô hạn.

Potential policies:

- fixed-size;

- quality-based replacement;

- diversity-based selection;

- canonical-only;

- prototype aggregation.

Memory capacity phải được xem là hyperparameter nếu dynamic memory được chọn.

# **46. Memory Selection**

Nếu:

Mi={mi1,...,mik}M_i=\\m\_{i1},...,m\_{ik}\\

thì retrieval có thể cần:

Select(Mi,st)Select(M_i,s_t)

Không nhất thiết mọi memory item đều được đưa vào model.

Possible selection criteria:

- viewpoint relevance;

- appearance compatibility;

- identity quality;

- pose relevance;

- recency;

- diversity.

Đây là design candidate, chưa khóa.

# **47. Scene Awareness Beyond Character Presence**

Minimum scene awareness là:

ci∈Ct?c_i\in C_t?

Advanced version có thể dùng:

- scene role;

- expected pose;

- camera;

- current appearance;

- interaction partner.

Nếu RESEARCH-02 cho thấy simple character presence đã đủ:

> không cần over-engineer retrieval.

# **48. Single-Character Mode**

Khi:

∣Ct∣=1\|C_t\|=1

method giảm thành:

Scene

\+

M_i

\+

A_i,t

→ Generation

Case này giúp xác định:

> identity memory bản thân có hiệu quả không

trước khi thêm complexity của multi-character.

# **49. Multi-Character Mode**

Khi:

∣Ct∣\>1\|C_t\|\>1

cần thêm:

Identity separation

Character binding

Isolation

Do đó experiment nên phân tích single-character và multi-character riêng.

# **50. Method Variants**

Để ablation rõ ràng, SAMIM có thể được triển khai theo incremental variants.

### **V0 — Baseline**

Không SAMIM.

### **V1 — Character-Specific Identity Memory**

+Mi+M_i

### **V2 — Memory + Scene-Aware Selection**

+Mi+Ct based retrieval+M_i + C_t\\ based\\ retrieval

### **V3 — Memory + Scene-Aware + Isolation**

thêm multi-character separation.

### **V4 — Full Candidate Method**

thêm explicit identity/appearance separation.

Final component ordering có thể thay đổi sau RESEARCH-02.

# **51. Core Ablation Targets**

Tối thiểu cần có khả năng loại riêng:

### **A1 — Remove Character Memory**

Không persistent MiM_i.

### **A2 — Remove Scene-Aware Retrieval**

Sử dụng global/shared memory.

### **A3 — Remove Identity Isolation**

Cho character conditions interaction như baseline integration.

### **A4 — Merge Identity and Appearance**

Không tách MiM_i khỏi Ai,tA\_{i,t}.

### **A5 — Remove Historical Memory Update**

Canonical references only.

Ablation cuối cùng được khóa trong RESEARCH-04.

# **52. Expected Effects**

Nếu hypotheses đúng:

### **Character Memory**

nên cải thiện long-range reappearance.

### **Scene-Aware Retrieval**

nên giảm irrelevant identity interference.

### **Identity Isolation**

nên giảm leakage/mixing.

### **Identity–Appearance Separation**

nên giữ recognition mà không giảm scene variation quá mức.

Đây là **hypotheses**, không phải expected results được đảm bảo.

# **53. Validation Layer**

Generated candidate cần được đánh giá ít nhất ở ba dimension:

Identity correctness

Identity separation

Scene compliance

Không được approve research success chỉ dựa trên identity similarity.

# **54. Per-Character Validation**

Với mỗi:

ci∈Ctc_i\in C_t

kiểm tra:

Is c_i present?

Is identity recognizable?

Does it match c_i rather than c_j?

Does scene appearance satisfy A_i,t?

# **55. Multi-Character Validation**

Với:

ci,cj∈Ctc_i,c_j\in C_t

cần kiểm tra cả:

Similarity(outputi,referencei)Similarity(output_i,reference_i)

và cross-similarity:

Similarity(outputi,referencej)Similarity(output_i,reference_j)

để phát hiện identity confusion.

Metric cụ thể thuộc RESEARCH-04.

# **56. Method Diagnostics**

Mỗi generation nên ghi:

active_character_ids

identity_memory_ids

identity_memory_versions

appearance_state_ids

retrieval_strategy

isolation_strategy

conditioning_strategy

method_version

model_version

seed

để có thể phân tích failure.

# **57. Research Provenance**

Generated candidate phải truy ngược được:

Image

↓

Generation

↓

Method Version

↓

Identity Memory

↓

Canonical Reference

↓

Scene Appearance

Nếu không biết candidate dùng memory nào thì không thể làm ablation đáng tin cậy.

# **58. Memory Integrity**

Identity memory phải có:

character_id

và nếu cần:

identity_version

Không được lookup bằng character name làm technical identity duy nhất.

# **59. Cross-Character Contamination Rule**

Memory của Project A / Character X không được xuất hiện trong condition của character khác do retrieval error.

Đây vừa là implementation correctness vừa là research validity.

# **60. Failure Handling**

Nếu không có valid identity memory cho active character:

không được âm thầm lấy một character “gần giống”.

Generation có thể:

FAIL

hoặc:

FALLBACK_TO_REFERENCE

tùy experimental policy.

Fallback phải được log.

# **61. Baseline Compatibility**

SAMIM phải được thiết kế để có thể so sánh trong cùng experiment infrastructure với baseline.

Ví dụ:

MethodAdapter

├── StoryDiffusion

├── DreamStory

├── StoryIter

└── SAMIM

Product workflow không thay đổi chỉ vì research method thay đổi. Đây cũng là boundary được system architecture hiện tại đề xuất.

# **62. Scope of Scientific Contribution**

Contribution của SAMIM không phải:

- Story Core;

- database;

- UI;

- background queue;

- Canon management;

- RAG narrative memory;

- story editor.

Những thành phần đó hỗ trợ application.

Research contribution phải nằm ở:

> **cách visual identity information được tổ chức, chọn và sử dụng để cải thiện long-range multi-character generation.**

# **63. Relationship with Application Story Memory**

Cần tách tuyệt đối hai khái niệm:

### **Narrative Memory**

Facts, events, knowledge, relationship, timeline.

### **Visual Identity Memory**

Representation dùng để giữ visual identity.

Không được dùng cùng từ “memory” rồi xem chúng là cùng một mechanism.

SAMIM nghiên cứu:

> **Visual Identity Memory.**

# **64. Relationship with Character Reference**

Character Reference là source asset.

Identity Memory là research representation.

Conceptually:

Reference→EidMiReference \xrightarrow{E\_{id}} M_i

Không nên đồng nhất:

Reference=MiReference=M_i

trừ khi final method lựa chọn raw reference memory.

# **65. Relationship with Visual State**

Visual State cung cấp:

Ai,tA\_{i,t}

Identity Memory cung cấp:

MiM_i

Image generation dùng cả hai:

(Mi,Ai,t)(M_i,A\_{i,t})

Nguồn application hiện tại cũng tách Profile/Reference khỏi Visual State và dùng cả hai khi tạo ảnh.

# **66. Research Decisions Pending**

Những decision sau **chưa được phép xem là đã chốt**:

| **Decision**                | **Status** |
|-----------------------------|------------|
| Representation của MiM_i    | Pending    |
| Encoder                     | Pending    |
| Static hay dynamic memory   | Pending    |
| Memory size                 | Pending    |
| Memory selection            | Pending    |
| Diffusion integration point | Pending    |
| Isolation mechanism         | Pending    |
| Mask/spatial binding        | Pending    |
| Memory update threshold     | Pending    |
| Previous appearance policy  | Pending    |
| Base diffusion model        | Pending    |

# **67. Decision Criteria**

Mỗi decision phải dựa trên ít nhất một trong:

1.  RESEARCH-02 failure evidence;

2.  implementation feasibility;

3.  controlled pilot experiment;

4.  ablation result;

5.  compute constraints.

Không chọn component chỉ vì:

> “Có vẻ hiện đại.”

# **68. Minimum Viable Research Method**

Nếu compute/time hạn chế, phiên bản tối thiểu cần giữ ba ý:

Per-character identity memory

\+

Scene-aware active-character selection

\+

Identity/appearance separation

Isolation mechanism nâng cao chỉ bắt buộc nếu baseline evidence chứng minh multi-character interference là failure đáng kể.

# **69. Method Implementation Stages**

### **Stage 1 — Single Character Prototype**

Chứng minh:

MiM_i

có thể giữ identity qua multiple scenes.

### **Stage 2 — Long-Range Reappearance**

Character biến mất rồi quay lại.

### **Stage 3 — Two-Character Generation**

Kiểm tra leakage.

### **Stage 4 — Appearance Variation**

Outfit/expression/pose thay đổi.

### **Stage 5 — Full Benchmark Integration**

Chạy protocol RESEARCH-04.

# **70. Stage 1 Success Condition**

Single character:

Reference

↓

M_i

↓

Different scene prompts

↓

Same recognizable character

Nếu Stage 1 thất bại:

> chưa nên thêm multi-character isolation.

# **71. Stage 2 Success Condition**

Long-gap cases cho thấy SAMIM không suy giảm mạnh hơn baseline khi reappearance gap tăng.

Statistical criterion thuộc RESEARCH-04.

# **72. Stage 3 Success Condition**

Hai character:

A≠BA\neq B

vẫn recognizable riêng biệt trong cùng scene.

Không đạt nếu:

- merge;

- swapped;

- leakage;

- one character disappears.

# **73. Stage 4 Success Condition**

Character giữ identity khi:

- outfit thay đổi;

- expression thay đổi;

- pose thay đổi;

- camera thay đổi;

- visual state thay đổi.

Không được giữ consistency chỉ bằng cách copy reference appearance.

# **74. Risks**

## **R1 — Identity Memory Overfitting**

Model quá phụ thuộc reference → ảnh thiếu variation.

## **R2 — Weak Identity Representation**

Memory không đủ → long-range drift.

## **R3 — Identity Leakage**

Multi-character condition bị trộn.

## **R4 — Dynamic Memory Corruption**

Generated error được ghi vào memory.

## **R5 — Memory Growth**

Dynamic memory tăng không kiểm soát.

## **R6 — Incorrect Scene Binding**

Memory đúng nhưng gán sai character.

## **R7 — Future-State Leakage**

Scene cũ dùng identity version tương lai.

## **R8 — Compute Overhead**

Method cải thiện nhỏ nhưng cost tăng quá lớn.

# **75. Risk Mitigation Candidates**

| **Risk**            | **Candidate mitigation**                  |
|---------------------|-------------------------------------------|
| Overfitting         | Identity/appearance separation            |
| Weak representation | Multi-reference / stronger representation |
| Leakage             | Isolation / spatial binding               |
| Memory corruption   | Canonical-only or validated update        |
| Memory growth       | Fixed budget                              |
| Wrong binding       | Stable Character ID                       |
| Future leakage      | Temporal identity version                 |
| Compute cost        | Memory compression / selective retrieval  |

Đây là design candidates, không phải final solution.

# **76. Method Success Definition**

SAMIM chỉ được xem là thành công nếu improvement không chỉ xảy ra trên một metric.

Cần hướng tới:

ΔIdentity\>0\Delta Identity \> 0

đồng thời:

ΔLeakage\<0\Delta Leakage \< 0

và:

SceneAlignmentSceneAlignment

không suy giảm đáng kể.

Nếu identity tăng nhưng image diversity hoặc scene compliance giảm mạnh:

> phương pháp chưa giải quyết đúng bài toán.

# **77. Research Traceability**

Mỗi component phải liên kết:

Observed Failure

↓

Hypothesis

↓

Method Component

↓

Experiment

↓

Metric

↓

Result

Ví dụ:

Long-range identity loss

↓

H1

↓

Character-Specific Memory

↓

Long-range experiment

↓

Identity metric

↓

Accept/Reject H1

# **78. Traceability Matrix**

| **Failure**                      | **Hypothesis** | **SAMIM Component**            |
|----------------------------------|----------------|--------------------------------|
| Long-range identity loss         | H1             | Per-Character Memory           |
| Irrelevant identity interference | H2             | Scene-Aware Retrieval          |
| Identity leakage                 | H3             | Identity Isolation             |
| Appearance over/under constraint | H4             | Identity–Appearance Separation |
| Error propagation                | Exploratory    | Controlled Memory Update       |

# **79. Relationship with RESEARCH-01**

RESEARCH-01 nói:

> chúng ta giả thuyết điều gì?

RESEARCH-03 nói:

> **nếu hypothesis đó có cơ sở, component kỹ thuật nào có thể trực tiếp giải quyết nó?**

# **80. Relationship with RESEARCH-02**

RESEARCH-02 có quyền thay đổi RESEARCH-03.

Nếu baseline evidence cho thấy:

Identity leakage ≈ insignificant

thì Identity Isolation không nhất thiết là core contribution.

Nếu:

Long-range drift = dominant failure

thì Memory Retrieval trở thành contribution chính.

# **81. Relationship with RESEARCH-04**

RESEARCH-04 phải kiểm tra:

Baseline

vs

Memory only

vs

Memory + Scene-Aware

vs

Memory + Scene-Aware + Isolation

vs

Full SAMIM

để biết component nào thực sự đóng góp.

Không được chỉ so:

Baseline vs Full Proposed

rồi kết luận mọi component đều hữu ích.

# **82. Final Method Principle**

Nguyên tắc trung tâm của SAMIM là:

> **Identity should belong to the character, not to the most recent scene.**

Và:

> **Only identities relevant to the current scene should participate in generation.**

Cùng với:

> **Character identity should remain stable while scene-dependent appearance remains mutable.**

Có thể tóm gọn toàn bộ phương pháp:

Character→Persistent Identity MemoryCharacter \rightarrow Persistent\\ Identity\\ Memory Scene→Active Character SelectionScene \rightarrow Active\\ Character\\ Selection Character+Scene→Current AppearanceCharacter + Scene \rightarrow Current\\ Appearance Identity+Appearance→Character-Aware ConditioningIdentity + Appearance \rightarrow Character\text{-}Aware\\ Conditioning Multiple Characters→Identity IsolationMultiple\\ Characters \rightarrow Identity\\ Isolation ↓\downarrow Consistent Story ImageConsistent\\ Story\\ Image

# **83. Final Status of Version 0.1**

RESEARCH-03 v0.1 **chốt kiến trúc logic**, nhưng chưa chốt kiến trúc neural cụ thể.

Đã chốt:

- per-character identity concept;

- scene-aware selection;

- long-range retrieval principle;

- identity/appearance separation;

- research/application boundary;

- traceability và ablation requirement.

Chưa chốt:

- encoder;

- embedding;

- attention modification;

- diffusion integration;

- static/dynamic memory;

- isolation implementation;

- memory update algorithm.

Những quyết định trên chỉ được khóa sau khi RESEARCH-02 cung cấp failure evidence và prototype experiments cho thấy phương án nào thực sự giải quyết failure đó.

Đây là nguyên tắc quan trọng nhất:

> **Do not turn an implementation idea into a scientific contribution until its necessity and effect have been demonstrated experimentally.**
