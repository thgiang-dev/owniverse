# **RESEARCH-04 — Experiment, Metrics & Ablation Protocol**

**Project:** Character-Consistent Long-Range Story Generation  
**Document ID:** RESEARCH-04  
**Document Type:** Experimental Evaluation Protocol  
**Version:** 0.1  
**Status:** Provisional — Must Be Frozen Before Final Evaluation  
**Parent Documents:** RESEARCH-01, RESEARCH-02, RESEARCH-03  
**Primary Purpose:** Xác định cách thực nghiệm, đo lường và kiểm chứng các giả thuyết nghiên cứu cũng như đóng góp của Scene-Aware Multi-Character Identity Memory.

# **1. Purpose**

RESEARCH-04 trả lời câu hỏi:

> **Làm thế nào để chứng minh một cách công bằng rằng phương pháp đề xuất thực sự cải thiện character consistency?**

Tài liệu định nghĩa:

- experimental conditions;

- baseline comparison;

- dataset sampling;

- long-range conditions;

- multi-character conditions;

- appearance variation conditions;

- metric groups;

- human evaluation;

- ablation study;

- reproducibility requirements;

- statistical comparison;

- hypothesis acceptance/rejection logic;

- reporting structure.

RESEARCH-04 không được điều chỉnh metric chính sau khi đã xem kết quả final experiment nếu không ghi rõ đó là post-hoc analysis.

# **2. Evaluation Principle**

Nguyên tắc tổng quát:

Same Data+Comparable Configuration+Same Evaluation ProtocolSame\\ Data + Comparable\\ Configuration + Same\\ Evaluation\\ Protocol ⇓\Downarrow BaselinevsProposed MethodBaseline \quad vs \quad Proposed\\ Method

Tài liệu nguồn quy định proposed method phải được đánh giá với baseline trên cùng dataset, experimental configuration và evaluation procedure.

# **3. Evaluation Dimensions**

Nguồn hiện tại xác định năm dimension chính:

1.  **Character Identity Preservation**

2.  **Multi-Character Distinguishability**

3.  **Scene Description Alignment**

4.  **Long-Range Reappearance Consistency**

5.  **Overall Image Quality**

RESEARCH-04 sử dụng năm dimension này làm evaluation backbone.

# **4. Benchmark**

Benchmark chính dự kiến:

> **ViStoryBench**

Theo tài liệu nguồn:

- 80 stories;

- 1,317 scenes;

- 344 characters;

- 509 character reference images;

- story length từ 4–30 scenes;

- trung bình khoảng 16.5 scenes.

Dataset hỗ trợ nghiên cứu recurring character, multi-character scenes, appearance variation và scene compliance.

# **5. Evaluation Philosophy**

Không có một metric duy nhất đủ để kết luận phương pháp tốt hơn.

Ví dụ:

Một method có thể đạt:

High Identity SimilarityHigh\\ Identity\\ Similarity

nhưng:

Low Scene AlignmentLow\\ Scene\\ Alignment

vì nó gần như copy character reference.

Hoặc:

High Image QualityHigh\\ Image\\ Quality

nhưng:

High Identity LeakageHigh\\ Identity\\ Leakage

Do đó final conclusion phải dựa trên **multi-dimensional evaluation**.

# **6. Main Evaluation Questions**

RESEARCH-04 phải trả lời tối thiểu:

### **EQ1**

Phương pháp đề xuất có giữ identity tốt hơn baseline không?

### **EQ2**

Improvement có còn tồn tại khi reappearance gap tăng không?

### **EQ3**

Phương pháp có giảm confusion/leakage giữa nhiều character không?

### **EQ4**

Character consistency tăng có làm giảm scene alignment không?

### **EQ5**

Character consistency tăng có làm giảm overall visual quality không?

### **EQ6**

Component nào của SAMIM thực sự đóng góp vào improvement?

# **7. Methods Under Comparison**

Core comparison:

M0 — StoryDiffusion

M1 — DreamStory

M2 — Story-Iter

M3 — SAMIM Proposed Method

StoryDiffusion là baseline chính; DreamStory và Story-Iter đóng vai trò comparators cho multi-subject và long-story generation theo định hướng hiện tại.

ConsiStory có thể được thêm làm auxiliary comparator nếu reproduction trong RESEARCH-02 cho thấy comparison hợp lệ.

# **8. Method Eligibility**

Một method chỉ được đưa vào final comparison nếu:

Reproduction Status = REPRODUCED

hoặc:

CONTROLLED_TEST_READY

theo RESEARCH-02.

Method không tái hiện thành công không được coi là baseline thất bại về mặt khoa học.

# **9. Experiment Phases**

Evaluation được chia thành bốn phase.

### **Phase A — Pilot**

Kiểm tra:

- pipeline;

- metric implementation;

- data extraction;

- resource consumption;

- experimental feasibility.

### **Phase B — Baseline Characterization**

Xác định baseline behavior và failure pattern.

### **Phase C — Proposed Method Evaluation**

So sánh SAMIM với baseline.

### **Phase D — Ablation & Human Evaluation**

Xác định component contribution và kiểm chứng các trường hợp metric tự động chưa phản ánh đủ.

# **10. No Final Conclusion from Pilot**

Pilot data có thể dùng để:

- sửa bug;

- chọn feasible configuration;

- estimate compute;

- kiểm tra metric sensitivity.

Pilot không được dùng như final test set nếu đã được sử dụng nhiều lần để tune phương pháp mà không khai báo.

# **11. Dataset Partitioning**

Dataset nên được phân tách logic thành:

Development / Diagnostic Set

Final Evaluation Set

Development set dùng cho:

- baseline analysis;

- prototype;

- debugging;

- method design;

- hyperparameter selection.

Final Evaluation Set dùng để:

> đánh giá phương pháp sau khi architecture/configuration đã được khóa.

Exact split:

> **TO BE FROZEN BEFORE FINAL EVALUATION**

# **12. Sampling Principle**

Final set phải bao phủ đủ:

- short stories;

- long stories;

- single recurring character;

- multiple recurring characters;

- multi-character scenes;

- long absence/reappearance;

- appearance changes;

- different visual styles nếu benchmark có.

Không chỉ chọn những story dễ hoặc đẹp.

# **13. Sampling Manifest**

Trước final experiment phải tạo:

evaluation_manifest.json

chứa:

story_id

scene_ids

character_ids

reference_ids

character_occurrence_positions

reappearance_gaps

character_count_per_scene

appearance_variation_tags

dataset_split

Manifest phải được khóa trước khi final result được phân tích.

# **14. Unit of Evaluation**

Có bốn levels.

## **Image-Level**

Một generated scene.

## **Character-Instance Level**

Một character xuất hiện trong một image.

## **Character-Trajectory Level**

Một character qua nhiều appearances.

## **Story-Level**

Consistency của toàn bộ story.

Không nên chỉ báo cáo image-level average.

# **15. Character Trajectory**

Với character:

cic_i

có appearances:

Ai={ai1,ai2,...,aik}A_i= \\a\_{i1},a\_{i2},...,a\_{ik}\\

trajectory evaluation đánh giá:

> identity của cic_i có ổn định xuyên suốt kk appearances không?

Đây là đơn vị đặc biệt quan trọng đối với long-range consistency.

# **16. Reappearance Gap**

Theo RESEARCH-02:

g(aij)=sceneIndex(aij)−sceneIndex(ai,j−1)−1g(a\_{ij}) = sceneIndex(a\_{ij}) - sceneIndex(a\_{i,j-1}) -1

Final evaluation phải lưu raw gg.

Ví dụ:

Scene 2 → Character A

Scene 8 → Character A

g=5g=5

# **17. Gap Buckets**

Để phân tích long-range behavior, raw gap có thể được chia thành:

Short Gap

Medium Gap

Long Gap

Nhưng threshold cụ thể:

> **TO BE FROZEN AFTER DATASET DISTRIBUTION ANALYSIS AND BEFORE FINAL METHOD COMPARISON**

Không được nhìn result của proposed method rồi chọn bucket có lợi nhất.

# **18. Character Count Condition**

Với scene:

nt=∣Ct∣n_t=\|C_t\|

ít nhất cần phân tích:

Single-character scenes

Multi-character scenes

Nếu dữ liệu đủ:

1 character

2 characters

3+ characters

được báo cáo riêng.

# **19. Appearance Variation Condition**

Mỗi recurring character appearance có thể được phân loại:

Low Variation

Medium Variation

High Variation

dựa trên scene requirement.

Ví dụ high variation:

- outfit change;

- pose change;

- emotion change;

- lighting change;

- viewpoint change;

- injury/state change.

Rule cụ thể phải được định nghĩa trước annotation chính.

# **20. Controlled Variables**

Khi method cho phép, giữ cố định:

- story;

- scene descriptions;

- character references;

- scene ordering;

- resolution;

- number of outputs;

- random seed policy;

- evaluation dataset;

- evaluation implementation.

# **21. Method-Specific Variables**

Không phải mọi method có cùng interface.

Do đó những khác biệt như:

- native prompt preprocessing;

- reference mechanism;

- attention implementation;

- internal context;

được phép khác nếu là bản chất của method.

Nhưng phải document trong Reproduction Manifest.

# **22. Random Seed Policy**

Generative output có randomness.

Vì vậy một condition không nên dựa vào duy nhất một generation.

Mỗi method/scene condition cần:

KK

generation attempts.

Exact:

K=?K = ?

phải được khóa trước final experiment dựa trên compute budget.

Minimum rule:

> tất cả compared methods phải nhận cùng số opportunities dưới condition tương ứng.

# **23. Seed Matching**

Nếu method hỗ trợ seed control:

Seed Set =

{s1, s2, ..., sK}

cùng seed set nên được sử dụng khi việc đó có ý nghĩa kỹ thuật.

Nếu một method không hỗ trợ tương đương:

seed_control = unavailable

phải được report.

# **24. Metric Groups**

Evaluation gồm sáu nhóm metric logic:

### **MG1 — Identity Preservation**

### **MG2 — Identity Separation / Leakage**

### **MG3 — Long-Range Robustness**

### **MG4 — Scene Alignment**

### **MG5 — Image Quality**

### **MG6 — Human Perceptual Evaluation**

Nguồn hiện tại quy định automatic evaluation có thể được bổ sung bằng user evaluation khi metric chưa phản ánh đầy đủ human visual perception.

# **25. MG1 — Identity Preservation**

Mục tiêu:

> Generated appearance của character có giữ cùng identity với canonical reference hay không?

Logical score:

Sid(ci,st)=Similarity(GeneratedCharacter(ci,st),Reference(ci))S\_{id}(c_i,s_t) = Similarity( GeneratedCharacter(c_i,s_t), Reference(c_i) )

Exact similarity backend:

> **METRIC IMPLEMENTATION TO BE SELECTED AND FROZEN**

RESEARCH-04 không giả định một embedding model cụ thể từ tài liệu nguồn.

# **26. Identity Trajectory Score**

Với character có KiK_i appearances:

TrajectoryIdentity(ci)=1Ki∑j=1KiSid(ci,aij)TrajectoryIdentity(c_i) = \frac{1}{K_i} \sum\_{j=1}^{K_i} S\_{id}(c_i,a\_{ij})

Có thể bổ sung:

- minimum identity score;

- variance;

- final appearance score;

để tránh average che giấu một failure nặng.

# **27. Identity Worst-Case**

Một story có thể average tốt nhưng một scene làm character biến thành người khác.

Do đó nên báo cáo:

MinIdentity(ci)=min⁡jSid(ci,aij)MinIdentity(c_i) = \min_j S\_{id}(c_i,a\_{ij})

như supplementary analysis.

# **28. MG2 — Identity Separation**

Khi:

ci≠cjc_i\neq c_j

generated character cic_i phải gần reference của chính nó hơn reference của cjc_j.

Desired:

Sim(Gi,Ri)\>Sim(Gi,Rj)Sim(G_i,R_i) \> Sim(G_i,R_j)

# **29. Identity Margin**

Định nghĩa logical margin:

Margini=Sim(Gi,Ri)−max⁡j≠iSim(Gi,Rj)Margin_i = Sim(G_i,R_i) - \max\_{j\neq i}Sim(G_i,R_j)

Nếu:

Margini\>0Margin_i\>0

generated instance giống đúng identity hơn identities khác.

Margin càng lớn:

> character separation càng tốt.

Exact embedding backend vẫn phải được freeze trước final evaluation.

# **30. Character Confusion**

Có thể định nghĩa confusion event khi:

Sim(Gi,Rj)≥Sim(Gi,Ri)Sim(G_i,R_j) \ge Sim(G_i,R_i)

với một:

j≠ij\neq i

Từ đó đo:

ConfusionRate=ConfusedInstancesTotalCharacterInstancesConfusionRate = \frac{ConfusedInstances} {TotalCharacterInstances}

# **31. Identity Leakage**

Leakage cần được đánh giá bằng kết hợp:

- automatic cross-character similarity;

- failure annotation;

- human evaluation nếu cần.

Không nên xem facial similarity metric duy nhất là đủ để nhận biết mọi dạng leakage như clothing/accessory transfer.

# **32. MG3 — Long-Range Robustness**

Mục tiêu:

> Identity performance thay đổi như thế nào khi reappearance gap tăng?

Phân tích:

SidvsgS\_{id} \quad vs \quad g

Một method mạnh về long-range nên có degradation nhỏ hơn khi gg tăng.

# **33. Long-Range Degradation**

Conceptual definition:

DLR=Scoreshort−ScorelongD\_{LR} = Score\_{short} - Score\_{long}

Lower:

DLRD\_{LR}

thường biểu thị khả năng giữ identity tốt hơn qua khoảng cách dài, với điều kiện overall identity score không thấp.

# **34. Long-Range Reporting**

Phải báo cáo ít nhất:

| **Gap** | **Identity** | **Confusion** | **Scene Alignment** |
|---------|--------------|---------------|---------------------|
| Short   |              |               |                     |
| Medium  |              |               |                     |
| Long    |              |               |                     |

Điều này giúp phát hiện method chỉ tốt ở nearby scenes.

# **35. MG4 — Scene Alignment**

Mục tiêu:

> Generated image có thực sự phản ánh scene description không?

Scene alignment cần xem:

- đúng active characters;

- đúng action;

- đúng basic scene content;

- đúng scene-specific appearance;

- đúng relevant environment/context.

Logical:

Sscene(It,Dt)S\_{scene}(I_t,D_t)

Exact automated implementation:

> **TO BE SELECTED AND FROZEN**

# **36. Character Presence Accuracy**

Đối với scene có:

CtC_t

kiểm tra:

- missing character;

- extra character;

- wrong identity;

- duplicate character.

Đây là metric bổ trợ rất quan trọng cho multi-character scene.

# **37. Appearance Compliance**

Với:

Ai,tA\_{i,t}

kiểm tra generated character có tuân thủ scene-specific state không.

Ví dụ:

winter coat

angry

left arm bandaged

wet hair

Identity tốt nhưng các state trên sai:

> không được xem là complete success.

# **38. MG5 — Image Quality**

Nguồn xác định overall image quality là một tiêu chí đánh giá chính.

Image quality có thể bao gồm:

- visual coherence;

- artifacts;

- malformed characters;

- composition;

- perceptual quality.

Exact automatic quality metric:

> **TO BE FROZEN**

Nếu automatic metric không đủ tin cậy, human rating có thể đóng vai trò quan trọng hơn.

# **39. Style Consistency**

ViStoryBench cũng có tiêu chí liên quan style consistency.

Style consistency có thể được report như supplementary metric.

Tuy nhiên:

> nó không phải scientific contribution chính của SAMIM trừ khi research scope sau này thay đổi.

# **40. Composite Score**

Không nên tạo một weighted total score quá sớm.

Ví dụ:

Score=0.5Identity+0.2Alignment+...Score= 0.5Identity+ 0.2Alignment+ ...

có thể che giấu trade-off.

Khuyến nghị:

> báo cáo các dimension riêng trước.

Composite score chỉ dùng nếu có rationale rõ ràng được định nghĩa trước.

# **41. Primary vs Secondary Metrics**

Trước final evaluation phải xác định:

### **Primary Metrics**

Trực tiếp kiểm tra hypotheses.

### **Secondary Metrics**

Dùng để phát hiện side effects.

Provisional mapping:

Primary:

Identity Preservation

Identity Separation

Long-Range Robustness

Scene Alignment

Secondary:

Overall Image Quality

Style Consistency

Efficiency

# **42. Metric Selection Criteria**

Một automatic metric chỉ nên được chọn nếu:

1.  phù hợp với research question;

2.  có thể áp dụng thống nhất cho baseline và proposed method;

3.  không sử dụng ground truth unavailable;

4.  có implementation reproducible;

5.  có đủ sensitivity với failure cần đo.

Nếu không:

> dùng human evaluation hoặc manual annotation hỗ trợ.

# **43. Metric Pilot Validation**

Trước final run, metric cần được thử trên một tập failure cases đã biết.

Ví dụ:

Same identity

Wrong identity

Identity leakage

Correct outfit

Wrong outfit

Nếu metric không phân biệt được các trường hợp rõ ràng:

> không nên dùng làm primary metric.

# **44. Human Evaluation**

Nguồn hiện tại cho phép human/user evaluation nhằm bổ sung những trường hợp automatic metric không phản ánh đầy đủ human perception.

Human evaluation đặc biệt hữu ích cho:

- identity recognizability;

- identity leakage;

- scene compliance;

- visual quality.

# **45. Human Evaluation Questions**

Người đánh giá có thể trả lời bốn câu chính.

### **Q1 — Identity**

> Nhân vật trong ảnh có giống cùng nhân vật tham chiếu không?

### **Q2 — Separation**

> Các nhân vật khác nhau có giữ được đặc điểm riêng và không bị trộn không?

### **Q3 — Scene**

> Ảnh có phù hợp với mô tả scene không?

### **Q4 — Quality**

> Chất lượng hình ảnh tổng thể có chấp nhận được không?

# **46. Human Rating Scale**

Có thể sử dụng ordinal scale, ví dụ:

1 — Very Poor

2 — Poor

3 — Acceptable

4 — Good

5 — Very Good

Scale cuối:

> **TO BE FROZEN BEFORE STUDY**

Không thay wording/range giữa quá trình evaluation.

# **47. Pairwise Preference**

Ngoài absolute rating, có thể dùng blind pairwise comparison:

Image A

vs

Image B

Question:

> Ảnh nào giữ character identity tốt hơn mà vẫn phù hợp scene?

Options:

A

B

Tie / No clear preference

Pairwise evaluation có thể dễ hơn absolute rating trong một số trường hợp.

# **48. Blind Evaluation**

Khi khả thi, human evaluator không nên biết:

A = Baseline

B = Proposed

Method label phải được ẩn hoặc randomize để giảm bias.

# **49. Image Order Randomization**

Presentation order của:

- baseline outputs;

- proposed outputs;

phải randomize.

Không luôn đặt proposed method bên phải hoặc cuối.

# **50. Evaluator Instructions**

Evaluator phải được cung cấp định nghĩa rõ:

> Identity khác Appearance.

Ví dụ:

Changing outfit:

> không đồng nghĩa identity inconsistency.

Different face/person:

> identity inconsistency.

Nếu không, rating có thể phạt đúng những variation mà research muốn cho phép.

# **51. Human Evaluation Sample**

Không bắt buộc human-rate toàn bộ generated dataset nếu quá lớn.

Có thể chọn representative subset bao phủ:

- short/long gap;

- single/multi character;

- appearance variation;

- failure-prone scenes.

Sampling rule phải được định nghĩa trước.

# **52. Inter-Rater Reliability**

Nếu có nhiều evaluator, nên kiểm tra mức độ agreement.

Exact statistical coefficient:

> **TO BE SELECTED BASED ON FINAL RATING TYPE**

Mục tiêu:

> biết human judgments có nhất quán hay không.

# **53. Main Experiment E1 — Overall Baseline Comparison**

### **Objective**

Đánh giá overall character consistency.

### **Compare**

StoryDiffusion

DreamStory

Story-Iter

SAMIM

### **Evaluate**

- identity;

- separation;

- scene alignment;

- image quality.

### **Output**

Main comparison table.

# **54. E1 Result Table**

| **Method**     | **Identity ↑** | **Separation ↑** | **Scene Alignment ↑** | **Image Quality ↑** |
|----------------|----------------|------------------|-----------------------|---------------------|
| StoryDiffusion |                |                  |                       |                     |
| DreamStory     |                |                  |                       |                     |
| Story-Iter     |                |                  |                       |                     |
| SAMIM          |                |                  |                       |                     |

Không điền một overall winner nếu trade-offs phức tạp mà không thảo luận.

# **55. Main Experiment E2 — Long-Range Reappearance**

### **Objective**

Kiểm tra H1.

### **Condition**

Recurring character với reappearance gaps khác nhau.

### **Compare**

Baseline

vs

SAMIM

### **Measure**

- identity score;

- identity degradation;

- failure rate;

- human identity rating nếu cần.

# **56. H1 Evaluation**

H1:

> Character-specific memory cải thiện long-range identity consistency.

Evidence hỗ trợ H1 nếu:

1.  SAMIM đạt identity preservation cao hơn baseline trong long-gap cases;

2.  improvement lặp lại trên nhiều character/story;

3.  scene alignment không suy giảm nghiêm trọng;

4.  result không chỉ đến từ vài outlier images.

Nếu không:

> H1 không được hỗ trợ hoặc chỉ được hỗ trợ một phần.

# **57. Main Experiment E3 — Multi-Character Separation**

### **Objective**

Kiểm tra H2/H3.

### **Conditions**

1 Character

2 Characters

3+ Characters if available

### **Measure**

- identity margin;

- confusion;

- leakage;

- missing character;

- human distinction score.

# **58. H2 Evaluation**

H2:

> Scene-aware retrieval giảm unnecessary identity interference.

Main comparison:

Global / Non-filtered Identity Context

vs

Scene-Aware Identity Selection

Nếu scene-aware selection không cải thiện separation/leakage:

> component không được xem là supported contribution.

# **59. H3 Evaluation**

H3:

> Character-specific isolation giúp duy trì multiple distinct identities.

Compare:

SAMIM without isolation

vs

SAMIM with isolation

Measure:

- cross-character confusion;

- identity margin;

- leakage frequency;

- per-character identity score.

# **60. Main Experiment E4 — Appearance Variation**

### **Objective**

Kiểm tra H4.

### **Conditions**

Character xuất hiện trong nhiều scene với:

- different outfits;

- poses;

- expressions;

- temporary states.

### **Goal**

Identity giữ được trong khi appearance thay đổi đúng.

# **61. H4 Evaluation**

H4 được hỗ trợ khi:

Identityseparated\>IdentitymergedIdentity\_{separated} \> Identity\_{merged}

hoặc consistency ổn định hơn,

đồng thời:

AppearanceComplianceseparatedAppearanceCompliance\_{separated}

không thấp hơn đáng kể.

Nếu identity cao vì method copy reference appearance và bỏ qua scene:

> H4 không được xem là thành công.

# **62. Main Experiment E5 — Secondary Character Robustness**

Nếu RESEARCH-02 xác nhận secondary character degradation:

### **Objective**

So sánh:

Primary recurring characters

vs

Secondary recurring characters

Measure:

- identity preservation;

- long-gap consistency;

- failure frequency.

Experiment này chỉ trở thành core experiment nếu failure được baseline analysis xác nhận.

# **63. Ablation Principle**

Nguồn hiện tại yêu cầu ablation để đánh giá contribution của từng component, đặc biệt:

- identity storage mechanism;

- scene-aware memory selection;

- identity integration into generation.

RESEARCH-03 còn đề xuất explicit identity/appearance separation và isolation như các component có thể kiểm tra.

# **64. Core Ablation Matrix**

Provisional variants:

A0 — Baseline

A1 — + Character Identity Memory

A2 — + Scene-Aware Retrieval

A3 — + Identity Isolation

A4 — + Identity / Appearance Separation

A5 — Full SAMIM

Exact sequence phải phản ánh final implementation.

# **65. Ablation Table**

| **Variant** | **Memory** | **Scene-Aware** | **Isolation** | **I/A Separation** |
|-------------|------------|-----------------|---------------|--------------------|
| A0          | ✗          | ✗               | ✗             | ✗                  |
| A1          | ✓          | ✗               | ✗             | ✗                  |
| A2          | ✓          | ✓               | ✗             | ✗                  |
| A3          | ✓          | ✓               | ✓             | ✗                  |
| A4/A5       | ✓          | ✓               | ✓             | ✓                  |

Nếu final method không dùng Isolation:

> remove component khỏi final ablation.

# **66. Component Removal Ablation**

Ngoài incremental build, có thể dùng leave-one-out:

Full SAMIM

Full - Memory

Full - Scene-Aware

Full - Isolation

Full - Identity/Appearance Separation

Điều này giúp xác định component nào thực sự cần thiết trong full architecture.

# **67. Memory Ablation**

Nếu RESEARCH-03 thử nhiều memory strategy:

Canonical Reference Only

vs

Reference + Previous Appearance

vs

Dynamic Validated Memory

có thể tạo sub-experiment.

Chỉ cần nếu dynamic memory trở thành final method candidate.

# **68. Retrieval Ablation**

Nếu Memory có nhiều items:

Use All Memory

vs

Recent Memory

vs

Scene-Relevant Memory

vs

Proposed Selection

Chỉ triển khai nếu final method thực sự có memory selection phức tạp.

Không tạo ablation cho component không tồn tại.

# **69. Integration Ablation**

Nếu identity information có nhiều integration candidate:

Integration A

Integration B

Proposed Integration

được so sánh trong pilot hoặc ablation.

Nguồn hiện tại đặc biệt yêu cầu kiểm tra contribution của cách tích hợp identity vào generation.

# **70. Efficiency Evaluation**

Nếu SAMIM tăng compute đáng kể, cần report:

- generation latency;

- GPU memory usage;

- memory storage;

- preprocessing cost.

Efficiency không nhất thiết là primary contribution nhưng cần biết cost của improvement.

# **71. Fairness of Ablation**

Mọi variant phải giữ giống nhau:

- base model;

- dataset;

- generation count;

- seed policy;

- resolution;

- prompt;

- reference assets;

trừ component đang bị ablate.

Nếu thay nhiều yếu tố cùng lúc:

> không thể kết luận component nào tạo improvement.

# **72. Statistical Unit**

Cần tránh xem hàng nghìn character instances trong cùng một story hoàn toàn độc lập nếu chúng có correlation.

Final statistical analysis nên xem xét:

- character;

- story;

- scene;

như các grouping levels thích hợp.

Exact model thống kê:

> **TO BE FROZEN BASED ON FINAL DATA STRUCTURE**

# **73. Paired Comparison**

Khi cùng scene được tạo bởi nhiều methods:

ScenetbaselineScene_t^{baseline}

và:

ScenetSAMIMScene_t^{SAMIM}

comparison nên ưu tiên paired analysis vì input scene giống nhau.

Điều này giảm variation do dataset.

# **74. Confidence Reporting**

Không chỉ report:

Identity = 0.82

nên report uncertainty:

mean

spread / confidence interval

sample count

Exact confidence procedure phải được xác định trước final analysis.

# **75. Effect Size**

Nếu difference nhỏ:

0.801 vs 0.803

dù statistical test có thể cho significance trên sample rất lớn, practical benefit có thể thấp.

Do đó final discussion nên xem:

> magnitude of improvement

chứ không chỉ p-value.

# **76. Statistical Significance**

Nếu statistical hypothesis testing được sử dụng:

alpha

test type

multiple comparison handling

phải được chọn trước final statistical analysis.

Không thử nhiều test rồi chỉ report test có lợi.

# **77. Practical Significance**

Một result được xem là meaningful khi:

- improvement đủ lớn để quan sát;

- lặp lại trên nhiều story/character;

- không phá scene alignment;

- không tạo cost phi lý;

- human evaluation nếu có cũng ủng hộ.

# **78. Hypothesis Decision States**

Không chỉ dùng:

SUPPORTED

REJECTED

nên cho phép:

SUPPORTED

PARTIALLY_SUPPORTED

NOT_SUPPORTED

INCONCLUSIVE

Ví dụ H1 có thể:

> supported only for long-gap cases.

Đó vẫn là result khoa học hợp lệ.

# **79. Multiple Metrics and Hypothesis Decision**

Một hypothesis không nên được support chỉ vì một metric tăng.

Ví dụ H4 phải xem đồng thời:

IdentityIdentity

và:

AppearanceComplianceAppearanceCompliance

Nếu:

Identity ↑

Appearance Compliance ↓↓↓

thì conclusion phải phản ánh trade-off.

# **80. Failure Rate Evaluation**

Ngoài average metrics, report:

FailureRate=FailedCasesTotalCasesFailureRate = \frac{FailedCases}{TotalCases}

theo taxonomy RESEARCH-02.

Ví dụ:

- identity drift;

- identity leakage;

- character merge;

- long-range loss;

- wrong appearance.

Failure rate giúp giải thích metric averages.

# **81. Severe Failure Rate**

Nên report riêng:

SevereFailureRateSevereFailureRate

vì một method có thể average tốt nhưng đôi lúc tạo failure không sử dụng được.

Đây là đặc biệt quan trọng với creative application.

# **82. Per-Story Reporting**

Ngoài overall mean, nên tạo distribution theo story.

Ví dụ:

Story 01 → SAMIM better

Story 02 → similar

Story 03 → baseline better

...

Điều này giúp tránh conclusion bị chi phối bởi vài stories.

# **83. Per-Character Reporting**

Tương tự, cần kiểm tra:

- main character;

- secondary character;

- frequently recurring character;

- sparse recurring character.

Nếu method chỉ cải thiện main character:

> phải report rõ.

# **84. Qualitative Results**

Luận văn cần representative figures.

Mỗi figure nên bao gồm:

Character Reference

Scene N

Baseline output

SAMIM output

Scene N+k

Baseline output

SAMIM output

và mô tả ngắn failure/improvement.

# **85. Qualitative Selection Rule**

Không cherry-pick.

Nên có:

- representative success;

- representative failure;

- difficult case;

- baseline better case nếu có.

Scientific discussion mạnh hơn khi report cả limitations.

# **86. Blind Qualitative Selection**

Nếu có thể, representative cases nên được chọn dựa trên:

- predefined criteria;

- metric percentile;

- random sample;

thay vì chỉ chọn ảnh đẹp nhất.

# **87. Reproducibility**

Nguồn hiện tại yêu cầu research experiment lưu tối thiểu:

- model identifier;

- model/checkpoint version;

- context snapshot;

- generation parameters;

- seed nếu applicable;

- software/method version.

RESEARCH-04 xem đây là requirement bắt buộc.

# **88. Experiment Record**

Mỗi generation:

experiment_id

method

method_version

model_id

checkpoint

story_id

scene_id

character_ids

references

context_snapshot

generation_parameters

seed

output_asset

metric_results

failure_annotations

# **89. Frozen Configuration**

Trước final experiment tạo:

FINAL_EVAL_CONFIG_V1

Sau khi freeze:

- không thay prompt template;

- không thay reference count;

- không thay memory size;

- không thay integration strength;

- không thay metric implementation;

mà không tạo experiment version mới.

# **90. Experiment Versioning**

Nếu phải sửa:

FINAL_EVAL_CONFIG_V1

→ invalidated / superseded

FINAL_EVAL_CONFIG_V2

→ new run

Không trộn output từ V1/V2 như cùng một experiment.

# **91. Exclusion Policy**

Chỉ exclude khi có reason rõ:

TECHNICAL_FAILURE

CORRUPTED_OUTPUT

MISSING_REFERENCE

PIPELINE_BUG

INVALID_INPUT

UNSUPPORTED_METHOD_CONDITION

Không exclude:

BAD_LOOKING_OUTPUT

LOW_SCORE_OUTPUT

UNFAVORABLE_RESULT

# **92. Missing Generation**

Nếu method technical failure ở một sample:

không âm thầm regenerate đến khi đẹp.

Retry policy phải giống RESEARCH-02/application generation protocol và được log.

# **93. Best-of-N Problem**

Nếu chọn:

> best image trong 8 generations

cho proposed method,

nhưng baseline chỉ có 1 generation:

comparison không công bằng.

Do đó selection rule phải giống nhau.

Ví dụ:

all outputs evaluated

hoặc:

same N + same predefined selection mechanism

# **94. User Selection Bias**

Product cho phép user review candidate.

Nhưng academic comparison không nên để researcher chọn manually image đẹp nhất của proposed method nếu baseline không được hưởng cơ chế tương tự.

Product evaluation và algorithm evaluation cần tách.

# **95. Evaluation of Canonical Reference Dependence**

Nếu proposed method dùng nhiều reference hơn baseline, cần:

- document;

- kiểm soát;

- hoặc tạo separate experiment.

Không được gọi improvement đó hoàn toàn do algorithm nếu input information budget khác nhau đáng kể.

# **96. Metric Leakage**

Evaluation model không nên trực tiếp reuse internal features của proposed method theo cách làm proposed method có lợi thế không công bằng nếu baseline được đánh giá khác.

Metric phải method-independent khi có thể.

# **97. Researcher Bias**

Nếu manual failure annotation do chính tác giả thực hiện:

nên có:

- clear annotation guideline;

- blinded method labels khi khả thi;

- second reviewer cho subset nếu khả thi.

# **98. Experiment Outputs**

RESEARCH-04 phải tạo:

Final Evaluation Manifest

Frozen Experiment Configuration

Raw Generation Outputs

Metric Results

Failure Annotation Results

Ablation Results

Human Evaluation Results

Statistical Analysis

Qualitative Case Gallery

Hypothesis Decision Table

# **99. Main Result Table**

Template:

| **Method**     | **Identity ↑** | **Separation ↑** | **Long-Range ↑** | **Scene Align. ↑** | **Quality ↑** |
|----------------|----------------|------------------|------------------|--------------------|---------------|
| StoryDiffusion |                |                  |                  |                    |               |
| DreamStory     |                |                  |                  |                    |               |
| Story-Iter     |                |                  |                  |                    |               |
| SAMIM          |                |                  |                  |                    |               |

# **100. Long-Range Table**

| **Method**     | **Short Gap** | **Medium Gap** | **Long Gap** | **Degradation ↓** |
|----------------|---------------|----------------|--------------|-------------------|
| StoryDiffusion |               |                |              |                   |
| DreamStory     |               |                |              |                   |
| Story-Iter     |               |                |              |                   |
| SAMIM          |               |                |              |                   |

# **101. Multi-Character Table**

| **Method**     | **1 Char** | **2 Char** | **3+ Char** | **Confusion ↓** | **Leakage ↓** |
|----------------|------------|------------|-------------|-----------------|---------------|
| StoryDiffusion |            |            |             |                 |               |
| DreamStory     |            |            |             |                 |               |
| Story-Iter     |            |            |             |                 |               |
| SAMIM          |            |            |             |                 |               |

# **102. Ablation Table**

| **Variant**       | **Identity** | **Long-Range** | **Separation** | **Alignment** | **Quality** |
|-------------------|--------------|----------------|----------------|---------------|-------------|
| Baseline          |              |                |                |               |             |
| \+ Memory         |              |                |                |               |             |
| \+ Scene-Aware    |              |                |                |               |             |
| \+ Isolation      |              |                |                |               |             |
| \+ I/A Separation |              |                |                |               |             |
| Full              |              |                |                |               |             |

# **103. Hypothesis Decision Table**

| **Hypothesis**                    | **Main Experiment** | **Decision** | **Evidence** |
|-----------------------------------|---------------------|--------------|--------------|
| H1 Character Memory               | E2                  |              |              |
| H2 Scene-Aware Retrieval          | E3/Ablation         |              |              |
| H3 Identity Isolation             | E3/Ablation         |              |              |
| H4 Identity/Appearance Separation | E4/Ablation         |              |              |

# **104. H1 Acceptance Logic**

H1 được hỗ trợ nếu Character Memory:

- cải thiện long-range identity;

- improvement lặp lại;

- không gây major scene-quality trade-off.

Nếu chỉ short-range tăng:

> PARTIALLY_SUPPORTED hoặc NOT_SUPPORTED cho long-range claim.

# **105. H2 Acceptance Logic**

H2 được hỗ trợ nếu Scene-Aware Retrieval:

- giảm irrelevant interference;

- giảm leakage/confusion;

- đặc biệt trong multi-character story;

- không làm mất active character information.

# **106. H3 Acceptance Logic**

H3 được hỗ trợ nếu Isolation:

- tăng identity margin;

- giảm confusion;

- giảm merge/leakage;

- không phá composition/scene.

# **107. H4 Acceptance Logic**

H4 được hỗ trợ nếu explicit identity/appearance separation:

- giữ identity;

- đồng thời tăng/giữ appearance compliance;

- không đơn giản freeze character appearance.

# **108. Negative Result Policy**

Nếu một component không cải thiện:

> report.

Nếu baseline tốt hơn:

> report.

Nếu hypothesis bị bác bỏ:

> đó vẫn là research result.

Không sửa hypothesis sau experiment để biến result thành “thành công”.

# **109. Scope Control**

Không cần biến RESEARCH-04 thành benchmark khổng lồ.

Minimum defensible evaluation cần:

1.  baseline comparison;

2.  long-range test;

3.  multi-character test;

4.  appearance test;

5.  ablation;

6.  human evaluation nếu automatic metrics không đủ.

Đó là lõi đủ trực tiếp cho thesis question.

# **110. Recommended Priority Under Limited Compute**

Nếu GPU/time hạn chế, ưu tiên theo thứ tự:

1\. StoryDiffusion baseline

2\. Full SAMIM

3\. Long-range experiment

4\. Multi-character experiment

5\. Core ablation

6\. Appearance variation

7\. DreamStory / Story-Iter full comparison

8\. Extended human study

Đây là **khuyến nghị triển khai**, không phải yêu cầu từ tài liệu nguồn.

Mục tiêu là giữ phần khoa học cốt lõi trước khi mở rộng breadth.

# **111. Experiment Freeze Checklist**

Trước final evaluation phải trả lời được:

\[ \] Final methods?

\[ \] Final dataset split?

\[ \] Final scene set?

\[ \] Final references?

\[ \] Final gap buckets?

\[ \] Final seed/run count?

\[ \] Final generation config?

\[ \] Final metrics?

\[ \] Final metric implementations?

\[ \] Final ablation variants?

\[ \] Final exclusion policy?

\[ \] Final statistical plan?

\[ \] Human evaluation protocol?

Nếu chưa:

> final experiment chưa nên bắt đầu.

# **112. Final Research Logic**

Toàn bộ RESEARCH-04:

Research QuestionResearch\\ Question ↓\downarrow HypothesisHypothesis ↓\downarrow Controlled ExperimentControlled\\ Experiment ↓\downarrow Automatic Metrics+Human EvaluationAutomatic\\ Metrics + Human\\ Evaluation ↓\downarrow AblationAblation ↓\downarrow Statistical + Practical AnalysisStatistical\\ +\\ Practical\\ Analysis ↓\downarrow Accept/Partially Support/RejectAccept / Partially\\ Support / Reject ↓\downarrow Scientific ConclusionScientific\\ Conclusion

# **113. Final Evaluation Principle**

Nguyên tắc quan trọng nhất:

> **The proposed method must be evaluated not by whether it produces attractive examples, but by whether it produces repeatable improvements under controlled conditions.**

Một vài ảnh đẹp không phải evidence.

Một metric tăng không phải toàn bộ evidence.

Một baseline failure không phải research gap nếu không lặp lại.

Một component không phải contribution nếu ablation không chứng minh được tác dụng.

Luận văn chỉ được phép kết luận mạnh khi:

Observed FailureObserved\\ Failure

được xác nhận bởi RESEARCH-02,

Proposed ComponentProposed\\ Component

được đặc tả bởi RESEARCH-03,

và:

Measured ImprovementMeasured\\ Improvement

được chứng minh bởi RESEARCH-04.

# **114. Relationship of the Four Research Documents**

Bộ nghiên cứu hoàn chỉnh:

### **RESEARCH-01**

> **What is the research problem and what do we hypothesize?**

### **RESEARCH-02**

> **Where and how do existing methods fail?**

### **RESEARCH-03**

> **What method do we propose to address the confirmed failures?**

### **RESEARCH-04**

> **How do we prove whether the proposed method actually works?**

Tạo thành chuỗi:

Problem→Evidence→Method→ProofProblem \rightarrow Evidence \rightarrow Method \rightarrow Proof

Đây là research backbone của luận văn **Character-Consistent Long-Range Story Generation**.
