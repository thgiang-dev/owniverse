# **RESEARCH-02 — Baseline Reproduction & Failure Analysis Protocol**

**Project:** Character-Consistent Long-Range Story Generation  
**Document ID:** RESEARCH-02  
**Document Type:** Baseline Reproduction & Failure Analysis Protocol  
**Version:** 0.1  
**Status:** Draft for Experimental Preparation  
**Research Area:** Story Visualization / Character-Consistent Image Generation  
**Parent Document:** RESEARCH-01 — Research Problem & Hypotheses  
**Related Documents:** RESEARCH-03, RESEARCH-04  
**Primary Purpose:** Tái hiện các phương pháp baseline dưới điều kiện có kiểm soát, xác định failure pattern và cung cấp bằng chứng thực nghiệm cho research gap.

# **1. Purpose**

RESEARCH-02 xác định cách nghiên cứu các phương pháp hiện tại trước khi xây dựng phương pháp đề xuất.

Mục tiêu không phải chứng minh baseline “kém”, cũng không phải chứng minh trước rằng Scene-Aware Multi-Character Identity Memory là cần thiết.

Mục tiêu là trả lời:

1.  Các baseline có thể được tái hiện đáng tin cậy hay không?

2.  Chúng hoạt động tốt trong trường hợp nào?

3.  Chúng thất bại trong trường hợp nào?

4.  Failure có liên quan đến long-range recurrence hay không?

5.  Failure có tăng khi xuất hiện nhiều character hay không?

6.  Identity leakage có thực sự xuất hiện không?

7.  Appearance variation ảnh hưởng tới identity như thế nào?

8.  Failure nào đủ ổn định và lặp lại để trở thành research gap?

Tài liệu gốc đã xác định quy trình nghiên cứu theo hướng **baseline-driven failure analysis**: tái hiện các phương pháp đại diện, đánh giá trên cùng dữ liệu, phân tích identity drift, character mixing, identity leakage và các lỗi liên quan rồi mới xây dựng phương pháp mới.

# **2. Research Principle**

Nguyên tắc trung tâm của RESEARCH-02:

> **Do not design the solution before establishing the failure.**

Nghĩa là:

BaselineBaseline ↓\downarrow ReproductionReproduction ↓\downarrow Observed FailureObserved\\ Failure ↓\downarrow Failure PatternFailure\\ Pattern ↓\downarrow Confirmed Research GapConfirmed\\ Research\\ Gap ↓\downarrow RESEARCH-03RESEARCH\text{-}03

RESEARCH-03 chỉ được phép khóa kiến trúc phương pháp sau khi RESEARCH-02 tạo đủ bằng chứng.

# **3. Baseline Set**

## **3.1 StoryDiffusion — Primary Baseline**

StoryDiffusion được chọn làm baseline chính.

Theo tài liệu hiện tại, StoryDiffusion sử dụng **Consistent Self-Attention** để chia sẻ information giữa các image trong sequence và được lựa chọn vì trực tiếp hướng tới long-range consistency.

Vai trò trong nghiên cứu:

> **Primary long-range consistency baseline.**

Các câu hỏi cần kiểm tra:

- Identity có duy trì khi character xuất hiện liên tục không?

- Identity có giảm khi character biến mất nhiều scene rồi xuất hiện lại?

- Multiple characters ảnh hưởng lẫn nhau như thế nào?

- Shared information có gây identity mixing không?

- Character phụ có ổn định như character chính không?

# **4. DreamStory — Multi-Character Comparator**

DreamStory tập trung trực tiếp vào multi-subject story visualization.

Tài liệu mô tả DreamStory sử dụng multimodal reference cùng masked-attention-related mechanisms để duy trì các subject riêng biệt và hạn chế pha trộn identity.

Vai trò:

> **Primary comparator for multi-character identity separation.**

Các câu hỏi:

- DreamStory xử lý hai hoặc nhiều character tốt đến mức nào?

- Có giảm leakage so với StoryDiffusion không?

- Khi character tái xuất hiện sau khoảng cách dài, identity còn giữ được không?

- Multi-subject isolation có đổi lại bằng scene alignment hoặc visual quality hay không?

# **5. Story-Iter — Long-Story Comparator**

Story-Iter sử dụng generation trước làm reference cho generation sau và khai thác global reference information nhằm hỗ trợ long-story visualization.

Vai trò:

> **Comparator for iterative/global-reference long-story generation.**

Câu hỏi chính:

- Global reference có đủ để duy trì nhiều recurring identities không?

- Character lâu không xuất hiện có bị suy giảm representation không?

- Thông tin của character thường xuất hiện có lấn át character ít xuất hiện không?

- Error có tích lũy theo generation history hay không?

# **6. ConsiStory — Auxiliary Reference Baseline**

ConsiStory cũng nằm trong nhóm phương pháp liên quan của tài liệu hiện tại nhưng task của nó gần hơn với consistent subject generation hơn là full long-story multi-character visualization.

Vì vậy, trong protocol này ConsiStory được xem là:

> **Auxiliary consistency reference**, không bắt buộc phải đóng vai trò baseline chính cho mọi experiment.

Nó có thể hữu ích để kiểm tra:

- single-character consistency;

- subject feature preservation;

- sự khác biệt giữa “consistent subject generation” và “long-range story consistency”.

Quyết định đưa ConsiStory vào bảng comparison chính sẽ được thực hiện sau khi xác định khả năng tái hiện công bằng.

# **7. Reproduction Strategy**

Baseline reproduction được chia thành hai phase.

## **Phase A — Faithful Reproduction**

Mục tiêu:

> Kiểm tra xem implementation của baseline có tái hiện được hành vi kỳ vọng của chính phương pháp đó hay không.

Trong Phase A:

- giữ configuration gốc hoặc gần với implementation được cung cấp nhất có thể;

- sử dụng model/checkpoint phù hợp với phương pháp;

- không tự thay attention mechanism;

- không thêm module của phương pháp đề xuất;

- không thay reference strategy tùy tiện;

- ghi rõ mọi modification cần thiết.

Phase A trả lời:

> **Baseline có được tái hiện đúng trước khi chúng ta đem nó đi so sánh hay chưa?**

## **Phase B — Controlled Comparison**

Sau khi baseline hoạt động hợp lệ, cùng một tập test story/scene được đưa qua các phương pháp có khả năng xử lý tương ứng.

Mục tiêu:

> Giảm tối đa những khác biệt không liên quan tới research question.

Các yếu tố chung nên được giữ giống nhau khi khả thi:

- story sample;

- scene description;

- active characters;

- character references;

- scene order;

- target resolution;

- number of generated samples;

- evaluation input;

- experiment logging.

Không ép hai phương pháp có kiến trúc khác nhau phải sử dụng parameter không tương thích chỉ để tạo cảm giác “giống nhau”.

# **8. Reproduction Fidelity Rule**

Mỗi baseline phải có một **Reproduction Manifest**.

Ví dụ:

Method

Version

Repository / implementation source

Checkpoint

Base model

Dependencies

Inference configuration

Resolution

Reference strategy

Prompt processing

Seed policy

Hardware

Modifications

Known deviations

Nếu implementation phải sửa:

Original behavior

→ Modification

→ Reason

→ Expected impact

Không được âm thầm patch baseline rồi vẫn gọi đó là reproduction nguyên bản.

# **9. Reproducibility Requirements**

Tài liệu hệ thống hiện tại yêu cầu generation phục vụ luận văn phải lưu tối thiểu:

- model identifier;

- checkpoint/model version;

- context snapshot;

- generation parameters;

- seed nếu có;

- software/method version.

RESEARCH-02 bổ sung yêu cầu mỗi experiment output phải truy được:

ExperimentExperiment ↓\downarrow MethodMethod ↓\downarrow ConfigurationConfiguration ↓\downarrow StoryStory ↓\downarrow SceneScene ↓\downarrow Generation AttemptGeneration\\ Attempt ↓\downarrow OutputOutput

Không được có generated image trong experimental set mà không biết nó được tạo bằng configuration nào.

# **10. Benchmark**

Benchmark chính dự kiến là **ViStoryBench**.

Tài liệu hiện tại ghi nhận benchmark gồm:

- 80 stories;

- 1,317 scenes;

- 344 characters;

- 509 character reference images;

- story dài từ 4 đến 30 scenes;

- trung bình khoảng 16.5 scenes.

Dataset có structured information về story, scene và characters, đồng thời phù hợp với các tình huống recurring character, multi-character và appearance variation.

RESEARCH-02 không bắt buộc phải chạy toàn bộ dataset ngay từ đầu.

Có thể sử dụng staged evaluation.

# **11. Experimental Dataset Stages**

## **Stage 0 — Smoke Test**

Một tập nhỏ nhằm kiểm tra:

- pipeline chạy được;

- reference load đúng;

- prompt đúng scene;

- output được lưu;

- metadata đầy đủ.

Kết quả Stage 0 không được dùng để kết luận research gap.

## **Stage 1 — Diagnostic Subset**

Chọn một subset có chủ đích nhằm bao phủ:

- single recurring character;

- long-gap recurrence;

- multi-character scenes;

- character disappearance/reappearance;

- appearance variation.

Stage này dùng để xây failure taxonomy.

## **Stage 2 — Validation Set**

Sau khi taxonomy ổn định, chạy trên tập lớn hơn để xác định:

> Failure vừa quan sát là pattern hay chỉ là anecdotal case.

Sampling và kích thước chính xác được khóa trong RESEARCH-04.

# **12. Unit of Analysis**

Một đơn vị phân tích cơ bản là:

(character, scene, generation)(character,\\ scene,\\ generation)

Ví dụ:

Character: Aria

Scene: 12

Method: StoryDiffusion

Generation attempt: 3

Với multi-character analysis có thể dùng:

(characteri, characterj, scene, generation)(character_i,\\ character_j,\\ scene,\\ generation)

để phân tích leakage/confusion giữa hai identity.

# **13. Character Appearance History**

Đối với mỗi recurring character:

cic_i

lưu ordered appearance sequence:

Ai={ai1,ai2,…,aik}A_i=\\a\_{i1},a\_{i2},\ldots,a\_{ik}\\

trong đó:

aija\_{ij}

là lần xuất hiện thứ jj của character cic_i.

Điều này cho phép nghiên cứu:

- first appearance;

- consecutive appearance;

- reappearance;

- long-range reappearance.

# **14. Reappearance Gap**

Định nghĩa:

g(aij)=sceneIndex(aij)−sceneIndex(ai,j−1)−1g(a\_{ij}) = sceneIndex(a\_{ij}) - sceneIndex(a\_{i,j-1}) -1

Nếu character xuất hiện:

Scene 2

và tiếp theo là:

Scene 8

thì:

g=8−2−1=5g = 8-2-1=5

tức character vắng mặt 5 scene.

RESEARCH-02 phải lưu **raw gap value**.

Chưa chia Short / Medium / Long bằng threshold cố định.

Threshold sẽ được quyết định trong RESEARCH-04 dựa trên dataset distribution và experiment design.

Điều này tránh chọn threshold sau khi đã nhìn thấy kết quả.

# **15. Character Count**

Mỗi scene cần lưu:

nt=∣Ct∣n_t=\|C_t\|

trong đó CtC_t là active character set của scene tt.

Ví dụ:

Scene 1 → 1 character

Scene 2 → 2 characters

Scene 3 → 3 characters

Metadata này cho phép kiểm tra:

> Failure có tăng theo số lượng character hay không?

Generation pipeline của hệ thống cũng đã dự kiến lưu character count, scene position và distance từ lần xuất hiện trước để phục vụ research analysis.

# **16. Baseline Input Record**

Mỗi scene test nên có normalized research record:

story_id

scene_id

scene_index

scene_description

active_characters

character_reference_ids

character_count

previous_appearance_scene

reappearance_gap

appearance_state

method

method_version

generation_attempt

seed

output_asset

Nếu một trường không tồn tại trong method gốc, phải đánh dấu:

NOT_APPLICABLE

không tự tạo dữ liệu giả để làm đầy record.

# **17. Failure Analysis Taxonomy**

Các failure được chia thành bảy nhóm chính.

# **18. F1 — Identity Drift**

Định nghĩa:

> Cùng một character thay đổi dần identity qua sequence mặc dù Canon không có identity-changing event.

Ví dụ:

Scene 1

→ face shape A

Scene 5

→ hơi khác

Scene 10

→ khác đáng kể

Scene 15

→ gần như character mới

Cần phân biệt identity drift với appearance variation hợp lệ.

# **19. F2 — Long-Range Identity Loss**

Xảy ra khi character xuất hiện tương đối đúng ở các scene gần nhau nhưng tái xuất hiện sai sau khoảng cách lớn.

Pattern:

Consistency(gsmall)\>Consistency(glarge)Consistency(g\_{small}) \> Consistency(g\_{large})

nếu pattern này lặp lại.

RESEARCH-02 chưa dùng pattern trên để kết luận thống kê; chỉ ghi nhận observational evidence.

# **20. F3 — Identity Leakage**

Identity information của:

cic_i

xuất hiện trên:

cjc_j

với:

i≠ji\neq j

Ví dụ:

- hairstyle của A xuất hiện trên B;

- facial identity của A bị chuyển sang B;

- distinctive accessory bị gắn nhầm;

- hai character bắt đầu giống nhau bất thường.

Đây là failure đặc biệt quan trọng đối với multi-character analysis.

Tài liệu ứng dụng cũng đã định nghĩa multi-character validation phải kiểm tra A và B cùng xuất hiện nhưng identity không bị trộn.

# **21. F4 — Identity Mixing / Character Merge**

Hai identity không chỉ leakage một attribute mà bị merge thành một visual subject khó phân biệt.

Ví dụ:

Expected:

A = black hair

B = blonde hair

Generated:

Both characters:

same face

similar hair

hybrid clothing

Failure này có thể mạnh hơn ordinary leakage.

# **22. F5 — Secondary Character Degradation**

Main character giữ consistency tốt nhưng recurring secondary characters không ổn định.

Đây là failure cần ghi riêng vì average metric có thể che giấu nó.

Ví dụ:

Main character score → high

Secondary character score → low

Overall average → acceptable

nhưng story vẫn không usable.

# **23. F6 — Appearance Over-Constraint**

Model giữ identity bằng cách giữ quá nhiều visual details không cần thiết.

Ví dụ scene yêu cầu:

> Character mặc winter coat.

nhưng model cứ giữ outfit mặc định từ reference.

Hoặc:

> Character tức giận, tóc ướt dưới mưa.

nhưng output lặp lại gần như cùng portrait/reference pose.

Character có thể rất giống reference nhưng scene variation thấp.

# **24. F7 — Appearance Under-Constraint**

Ngược lại:

Scene-dependent variation quá mạnh làm identity bị mất.

Ví dụ:

Outfit change

\+

camera angle change

\+

expression change

\+

lighting change

khiến model tạo gần như một người khác.

Đây là failure trực tiếp liên quan H4 trong RESEARCH-01.

# **25. F8 — Scene Alignment Failure**

Image giữ identity tốt nhưng:

- sai character;

- thiếu character;

- sai action;

- sai scene intent;

- sai appearance state;

- sai location/visual requirement.

Không được tính một ảnh là successful character consistency nếu nó đạt identity bằng cách bỏ qua scene.

# **26. Failure Annotation Record**

Mỗi failure record nên có:

failure_id

story_id

scene_id

method

generation_attempt

character_id

failure_category

severity

evidence

reference_asset

previous_character_output

reappearance_gap

character_count

notes

review_status

Trong giai đoạn đầu:

review_status =

UNREVIEWED

REVIEWED

DISPUTED

CONFIRMED

# **27. Failure Severity**

Đề xuất ba mức.

### **Minor**

Khác biệt nhỏ nhưng character vẫn dễ nhận biết.

### **Moderate**

Identity hoặc appearance có vấn đề rõ ràng nhưng scene vẫn usable sau review.

### **Severe**

Character bị:

- mất identity;

- nhầm character;

- merge;

- leakage mạnh;

- hoặc scene không còn đạt mục tiêu.

Severity trong RESEARCH-02 dùng cho failure analysis, không thay thế metric chính thức của RESEARCH-04.

# **28. Failure Attribution**

Không phải mọi ảnh xấu đều là lỗi của character consistency method.

Phải phân biệt:

### **Method-related**

Có khả năng liên quan tới mechanism nghiên cứu.

### **Prompt-related**

Prompt thiếu hoặc mơ hồ.

### **Reference-related**

Reference không đủ hoặc không rõ.

### **Pipeline-related**

Bug preprocessing, wrong character assignment, wrong image loading.

### **Random generation failure**

Một sample riêng lẻ lỗi nhưng không tạo pattern.

### **Unknown**

Chưa xác định được nguyên nhân.

Nếu một lỗi pipeline bị nhầm thành model failure, research gap sẽ sai.

# **29. Pre-Analysis Validation**

Trước khi phân tích một output, phải kiểm tra:

1.  đúng method;

2.  đúng story;

3.  đúng scene;

4.  đúng character references;

5.  đúng active characters;

6.  đúng configuration;

7.  generation không crash;

8.  output asset hợp lệ;

9.  metadata đầy đủ.

Nếu không:

> exclude from research failure analysis và ghi technical exclusion reason.

# **30. Fair Comparison Principle**

Không được làm cho proposed method được hưởng nhiều information hơn baseline mà không khai báo.

Ví dụ nếu proposed method nhận:

- 3 character references;

- complete appearance state;

- manually cleaned prompt;

trong khi baseline chỉ nhận:

- 1 reference;

- raw scene sentence;

thì comparison không còn trả lời câu hỏi về algorithm alone.

Mọi khác biệt input phải được ghi trong comparison table.

# **31. Native Capability vs Controlled Input**

Một khó khăn là các phương pháp không có interface giống hệt nhau.

Vì vậy cần phân biệt:

### **Native Evaluation**

Method được sử dụng theo cách nó được thiết kế.

### **Controlled Evaluation**

Input được chuẩn hóa tối đa để so sánh một research condition chung.

Cả hai đều có giá trị nhưng trả lời câu hỏi khác nhau.

Không được trộn kết quả Native và Controlled trong cùng một bảng mà không ghi rõ.

# **32. Randomness Control**

Generative models có randomness.

Vì vậy không được kết luận:

> Method A tốt hơn Method B

dựa trên một ảnh duy nhất.

Mỗi condition cần nhiều generation attempts theo protocol RESEARCH-04.

RESEARCH-02 phải từ đầu lưu:

attempt_id

seed

nếu method hỗ trợ seed.

Nếu không hỗ trợ deterministic seed, phải ghi:

seed_control = unavailable

# **33. Qualitative Failure Review**

Trong giai đoạn failure discovery, review trực quan vẫn rất quan trọng.

Reviewer cần xem đồng thời:

Character reference

Previous appearance

Current scene requirement

Generated image

không chỉ xem generated image riêng lẻ.

Câu hỏi review:

1.  Có đúng character không?

2.  Có còn recognizable không?

3.  Có bị lẫn với character khác không?

4.  Scene-specific changes có hợp lý không?

5.  Có giữ những attribute đáng lẽ phải thay đổi không?

6.  Có thay những attribute đáng lẽ phải ổn định không?

7.  Scene intent có được giữ không?

# **34. Failure Case Package**

Mỗi case quan trọng nên được lưu thành một package:

Case ID

Story

Scene

Character references

Previous appearance(s)

Current generated outputs

\- StoryDiffusion

\- DreamStory

\- Story-Iter

Scene description

Observed failure

Relevant metadata

Package này rất hữu ích cho:

- luận văn;

- discussion;

- figure;

- supervisor review;

- RESEARCH-03 design.

# **35. Failure Frequency**

RESEARCH-02 cần phân biệt:

### **Isolated Failure**

Chỉ xuất hiện ở một số generation ngẫu nhiên.

### **Recurrent Failure**

Lặp lại nhiều attempts hoặc nhiều story.

### **Systematic Failure**

Liên quan rõ với một condition, ví dụ:

large reappearance gap

multi-character scenes

high appearance variation

Research gap nên ưu tiên **recurrent hoặc systematic failures**.

# **36. Research Gap Evidence Standard**

Một observation không trở thành research gap chỉ vì:

> “Tôi thấy ảnh này bị sai mặt.”

Một candidate research gap phải có tối thiểu:

### **Condition**

Xác định rõ tình huống gây failure.

### **Repetition**

Failure xuất hiện trên nhiều output/case.

### **Baseline relevance**

Failure xuất hiện ở ít nhất baseline quan trọng hoặc có bằng chứng rõ rằng baseline chính chưa giải quyết tốt.

### **Research relevance**

Failure trực tiếp liên quan RQ/Hypothesis của RESEARCH-01.

### **Measurability**

Có khả năng thiết kế metric hoặc experiment kiểm chứng.

# **37. Example Candidate Gap A**

Observation:

> Character consistency giảm khi reappearance gap tăng.

Nếu chỉ có 1 case:

> Anecdotal observation.

Nếu lặp trên nhiều characters/stories:

> Candidate pattern.

Nếu controlled experiment sau đó xác nhận:

> Validated research gap.

Sau đó RESEARCH-03 mới có lý do thiết kế persistent character-specific memory.

# **38. Example Candidate Gap B**

Observation:

> Khi Scene có A + B, facial identity của A thường xuất hiện trên B.

Nếu failure lặp lại:

> Multi-character interference candidate.

Điều này có thể dẫn tới nghiên cứu:

> Character-specific identity isolation.

Nhưng chỉ sau khi evidence tồn tại.

# **39. Example Candidate Gap C**

Observation:

> Model giữ reference rất tốt nhưng không chịu đổi outfit/expression.

Đây không phải success hoàn toàn.

Candidate research gap:

> Existing identity preservation may over-constrain scene-dependent appearance.

RESEARCH-03 khi đó cần giải quyết identity–appearance separation, không chỉ tăng similarity.

# **40. Baseline Failure Matrix**

Sau Phase 1, tạo matrix:

| **Condition**               | **StoryDiffusion** | **DreamStory** | **Story-Iter** |
|-----------------------------|--------------------|----------------|----------------|
| Single character, short gap |                    |                |                |
| Single character, long gap  |                    |                |                |
| Two recurring characters    |                    |                |                |
| Multi-character same scene  |                    |                |                |
| Appearance change           |                    |                |                |
| Character re-entry          |                    |                |                |
| Secondary character         |                    |                |                |

Không điền bằng cảm nhận chung.

Mỗi cell phải liên kết tới experiment records.

# **41. Failure Profile per Method**

Mỗi baseline sau cùng phải có profile dạng:

### **Strengths**

Những condition method xử lý tốt.

### **Weaknesses**

Những failure lặp lại.

### **Boundary Conditions**

Method bắt đầu suy giảm ở đâu.

### **Failure Examples**

Representative cases.

### **Unknowns**

Những điều dữ liệu hiện tại chưa đủ kết luận.

# **42. StoryDiffusion Failure Questions**

Đặc biệt kiểm tra:

- consistency có phụ thuộc scene gần nhất không;

- recurrence gap có ảnh hưởng không;

- multiple identities có bị share quá mức không;

- character nào được xuất hiện thường xuyên có dominate attention không;

- secondary character có drift nhanh hơn không.

Đây là câu hỏi nghiên cứu, chưa phải assertion rằng StoryDiffusion chắc chắn có các lỗi này.

# **43. DreamStory Failure Questions**

Kiểm tra:

- masked/multi-subject mechanism có giảm leakage hiệu quả không;

- nhiều recurring characters có giữ được distinction qua time không;

- long-range reappearance có yếu hơn immediate multi-subject consistency không;

- scene-specific appearance variation có gây identity loss không.

# **44. Story-Iter Failure Questions**

Kiểm tra:

- global reference có giữ được identity của character vắng mặt lâu không;

- iterative propagation có tích lũy error không;

- reference từ generation trước có truyền lỗi xuống scene sau không;

- nhiều identity có bị representation dilution không.

# **45. Negative Findings**

RESEARCH-02 phải cho phép kết luận:

> “Không phát hiện failure như dự kiến.”

Ví dụ nếu DreamStory xử lý identity leakage rất tốt:

Không được cố tìm cách chứng minh nó vẫn kém.

Thay vào đó phải điều chỉnh research gap.

Điều này có thể khiến RESEARCH-03 chuyển trọng tâm sang:

- long-range retrieval;

- secondary character consistency;

- identity–appearance separation;

- hoặc một vấn đề khác có evidence mạnh hơn.

# **46. No Cherry-Picking Rule**

Không được:

- chỉ chọn ảnh baseline xấu nhất;

- chỉ chọn ảnh proposed method đẹp nhất;

- bỏ failure case không thuận lợi cho giả thuyết;

- thay seed cho baseline nhưng không cho proposed;

- regenerate baseline ít lần hơn;

- thay prompt riêng cho từng method mà không khai báo.

Mọi experimental selection rule phải được định nghĩa trước ở RESEARCH-04.

# **47. Exclusion Criteria**

Output chỉ được exclude khi có lý do hợp lệ như:

- corrupted image;

- technical crash;

- wrong input due to pipeline bug;

- unsupported scene by method;

- missing reference asset;

- incomplete generation.

Không được exclude chỉ vì:

> “ảnh này làm kết quả baseline trông tốt hơn/xấu hơn.”

Mọi exclusion phải có reason code.

# **48. Suggested Exclusion Codes**

EX-TECHNICAL

EX-MISSING-ASSET

EX-INVALID-INPUT

EX-PIPELINE-BUG

EX-UNSUPPORTED-CONDITION

EX-CORRUPTED-OUTPUT

# **49. Experiment Metadata**

Tài liệu application hiện tại đã dự kiến lưu:

- recurring character IDs;

- character count;

- scene position;

- distance from previous appearance;

- generation attempt;

- consistency validation results.

RESEARCH-02 sử dụng trực tiếp metadata này.

Bổ sung:

experiment_id

baseline_method

method_version

datase
