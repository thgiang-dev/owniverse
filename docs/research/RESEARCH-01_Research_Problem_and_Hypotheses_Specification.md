# **RESEARCH-01 — Research Problem & Hypotheses Specification**

**Project:** Character-Consistent Long-Range Story Generation  
**Document ID:** RESEARCH-01  
**Document Type:** Research Problem & Hypothesis Specification  
**Version:** 0.1  
**Status:** Draft for Baseline Validation  
**Research Area:** Story Visualization / Generative AI / Character-Consistent Image Generation  
**Related Documents:** RESEARCH-02, RESEARCH-03, RESEARCH-04  
**Primary Purpose:** Xác định bài toán nghiên cứu, research gap, câu hỏi nghiên cứu và các giả thuyết cần kiểm chứng bằng thực nghiệm.

# **1. Purpose**

Tài liệu này xác định nền tảng khoa học của đề tài **Character-Consistent Long-Range Story Generation**.

Mục tiêu chính là trả lời bốn câu hỏi:

1.  Bài toán nghiên cứu chính xác là gì?

2.  Những hạn chế nào của các phương pháp hiện tại cần được kiểm chứng?

3.  Phương pháp nghiên cứu dự kiến cải thiện những khía cạnh nào?

4.  Những giả thuyết nào phải được kiểm chứng hoặc bác bỏ bằng thực nghiệm?

RESEARCH-01 không định nghĩa chi tiết kiến trúc của phương pháp đề xuất và không cố định metric hoặc cấu hình thí nghiệm.

Các nội dung đó lần lượt thuộc:

**RESEARCH-02  
**→ tái hiện baseline và xác định failure thực tế.

**RESEARCH-03  
**→ đặc tả phương pháp đề xuất sau khi research gap được xác nhận.

**RESEARCH-04  
**→ định nghĩa experiment, metric, ablation và tiêu chuẩn kết luận.

# **2. Research Context**

Các mô hình text-to-image hiện đại có khả năng tạo ảnh chất lượng cao từ mô tả ngôn ngữ tự nhiên. Tuy nhiên, phần lớn quá trình sinh ảnh vẫn có xu hướng xử lý từng ảnh như một generation riêng biệt.

Trong bài toán **story visualization**, một câu chuyện được biểu diễn bởi một chuỗi nhiều scene. Cùng một character có thể xuất hiện ở Scene 1, biến mất trong nhiều scene và chỉ tái xuất hiện ở Scene 10 hoặc Scene 20.

Trong trường hợp đó, hệ thống không chỉ phải tạo được một ảnh đẹp tại từng scene mà còn phải bảo đảm người xem vẫn nhận ra:

> Character xuất hiện tại Scene 20 chính là character đã xuất hiện trước đó.

Tài liệu gốc xác định ba vấn đề trung tâm: duy trì identity khi character tái xuất hiện sau khoảng cách dài, duy trì nhiều identity riêng biệt trong cùng story, và hạn chế identity leakage giữa các character.

# **3. Core Research Problem**

Bài toán nghiên cứu trung tâm được xác định như sau:

> **Làm thế nào để duy trì định danh thị giác riêng biệt và ổn định của nhiều nhân vật xuyên suốt một câu chuyện hình ảnh dài, đồng thời cho phép ngoại hình và trạng thái của nhân vật thay đổi phù hợp với nội dung của từng scene?**

Bài toán này bao gồm đồng thời ba yêu cầu.

### **3.1 Long-Range Identity Consistency**

Identity của character cần được duy trì ngay cả khi character không xuất hiện liên tục.

Ví dụ:

Scene 01  
→ Character A xuất hiện.

Scene 02–09  
→ Character A vắng mặt.

Scene 10  
→ Character A xuất hiện trở lại.

Khoảng cách giữa hai lần xuất hiện không được khiến hệ thống mất hoặc thay đổi identity của A.

Tình huống tái xuất hiện sau nhiều scene cũng đã được xác định là một workflow quan trọng của hệ thống.

### **3.2 Multi-Character Identity Separation**

Trong một story có nhiều recurring characters:

C={c1,c2,…,cN}C=\\c_1,c_2,\ldots,c_N\\

hệ thống không chỉ cần giữ từng character nhất quán mà còn phải duy trì khoảng cách identity giữa các character.

Nếu:

ci≠cjc_i \neq c_j

thì hệ thống phải hạn chế trường hợp visual information của cic_i được truyền nhầm sang cjc_j.

Một hệ thống tạo ra tất cả character khá nhất quán nhưng khiến chúng ngày càng giống nhau vẫn không được xem là giải quyết tốt bài toán.

### **3.3 Identity Preservation vs. Appearance Variation**

Character consistency không có nghĩa là character phải có ngoại hình giống hệt nhau trong mọi scene.

Cần phân biệt:

IdentityIdentity

với:

Scene-dependent AppearanceScene\text{-}dependent\\ Appearance

Identity có thể bao gồm các đặc trưng giúp người xem nhận biết character.

Appearance phụ thuộc scene có thể bao gồm:

- outfit;

- expression;

- pose;

- hairstyle variation hợp lệ;

- injury;

- lighting;

- action;

- scene-specific condition.

Tài liệu gốc nhấn mạnh rằng mục tiêu không phải tạo một character hoàn toàn giống nhau trong mọi ảnh, mà phải duy trì đủ đặc trưng nhận dạng trong khi vẫn cho phép appearance thay đổi theo nội dung.

# **4. Observed Failure Categories**

Nghiên cứu tập trung đặc biệt vào các dạng failure sau.

| **Failure**                         | **Mô tả**                                                                                            |
|-------------------------------------|------------------------------------------------------------------------------------------------------|
| **Identity Drift**                  | Identity của cùng một character thay đổi dần qua các scene                                           |
| **Long-Range Identity Loss**        | Character tái xuất hiện sau khoảng cách dài nhưng không còn giống reference hoặc appearance trước đó |
| **Identity Leakage**                | Đặc điểm của character này xuất hiện trên character khác                                             |
| **Identity Mixing**                 | Hai hoặc nhiều character bị pha trộn visual features                                                 |
| **Secondary Character Degradation** | Main character tương đối ổn định nhưng character phụ bị thay đổi mạnh                                |
| **Appearance Over-Constraint**      | Character giữ consistency bằng cách gần như không cho appearance thay đổi                            |
| **Appearance Under-Constraint**     | Scene variation quá mạnh đến mức làm mất identity                                                    |
| **Scene Misalignment**              | Character consistency được giữ nhưng appearance/action lại không phù hợp với scene                   |

Đây cũng là các nhóm lỗi mà baseline-driven failure analysis trong tài liệu hiện tại dự kiến khảo sát.

# **5. Related Methodological Directions**

Các phương pháp hiện tại cung cấp một số hướng giải quyết khác nhau.

**ConsiStory** cho thấy feature sharing giữa nhiều image có thể cải thiện subject consistency.

**StoryDiffusion** sử dụng Consistent Self-Attention để chia sẻ thông tin giữa các ảnh trong sequence và được chọn làm baseline chính cho bài toán long-range consistency.

**DreamStory** tập trung vào multi-subject story visualization và sử dụng cơ chế nhằm hạn chế sự pha trộn thông tin giữa nhiều subject.

**Story-Iter** sử dụng reference từ các generation trước và global information để hỗ trợ long story visualization.

Các phương pháp này cho thấy sharing, reference, masked attention và global information đều có thể hỗ trợ consistency.

Tuy nhiên, vấn đề cần kiểm chứng là liệu những cơ chế đó có đủ để đồng thời xử lý:

Long Range+Multi Character+Identity Separation+Appearance VariationLong\\ Range + Multi\\ Character + Identity\\ Separation + Appearance\\ Variation

hay không.

Tài liệu hiện tại xác định chính sự kết hợp của các yêu cầu này là vùng cần tiếp tục khảo sát.

# **6. Preliminary Research Gap**

Research gap hiện tại được đặt dưới dạng **giả định cần kiểm chứng**, chưa được xem là kết luận cuối cùng.

Giả định ban đầu là:

> Các phương pháp hiện tại có thể duy trì consistency trong một số tình huống, nhưng chưa chắc duy trì hiệu quả nhiều identity riêng biệt khi character xuất hiện không liên tục trong một sequence dài.

Một vấn đề tiềm năng khác là nhiều phương pháp dựa vào:

- recent images;

- shared feature;

- global representation;

- hoặc reference chung.

Điều này đặt ra câu hỏi:

> Nếu story chứa nhiều recurring characters với lịch sử xuất hiện khác nhau, liệu một global hoặc recent-context representation có đủ để phục hồi đúng identity của từng character hay không?

Hướng nghiên cứu hiện tại vì vậy xem xét việc duy trì một representation riêng:

M={M1,M2,…,MN}M=\\M_1,M_2,\ldots,M_N\\

trong đó:

MiM_i

đại diện cho identity information liên quan tới character:

cic_i

Tại scene sts_t, nếu chỉ có:

Ct⊆CC_t \subseteq C

xuất hiện, hệ thống dự kiến chỉ cần truy xuất identity information của các character thuộc CtC_t. Đây là định hướng đã được xác định trong tài liệu gốc.

**Lưu ý:** RESEARCH-01 chưa khẳng định cấu trúc MiM_i là embedding, image feature, token, reference bank hay dạng representation nào khác. Quyết định đó thuộc RESEARCH-03 và phải dựa trên kết quả của RESEARCH-02.

# **7. Main Research Question**

### **RQ0 — Primary Research Question**

> **How can distinct visual identities of multiple recurring characters be preserved across long-range story visualization while allowing scene-dependent appearance variation?**

Đây là research question cấp cao nhất của luận văn.

# **8. Research Sub-Questions**

### **RQ1 — Long-Range Reappearance**

> Khoảng cách giữa các lần xuất hiện ảnh hưởng như thế nào đến character identity consistency?

Nghiên cứu cần kiểm tra liệu identity score có suy giảm khi khoảng cách:

d(ci,t)d(c_i,t)

giữa lần xuất hiện hiện tại và lần xuất hiện trước của character tăng lên hay không.

### **RQ2 — Multi-Character Interference**

> Khi số lượng recurring characters hoặc số character cùng xuất hiện tăng lên, mức độ identity leakage có tăng hay không?

RQ2 tập trung vào interaction giữa các identity.

### **RQ3 — Identity Memory**

> Việc duy trì representation riêng cho từng character có cải thiện khả năng phục hồi identity sau khoảng cách dài hay không?

Đây là câu hỏi trực tiếp dẫn tới hướng Scene-Aware Multi-Character Identity Memory.

### **RQ4 — Scene-Aware Retrieval**

> Việc chỉ sử dụng identity information của những character thực sự xuất hiện trong scene có giảm interference giữa nhiều character hay không?

### **RQ5 — Identity–Appearance Separation**

> Việc tách thông tin identity tương đối ổn định khỏi scene-dependent appearance có giúp character vừa recognizable vừa tuân thủ nội dung scene tốt hơn hay không?

### **RQ6 — Trade-off**

> Việc tăng character consistency có làm giảm scene diversity, prompt alignment hoặc visual quality hay không?

Một phương pháp mới không được xem là thành công nếu identity score tăng nhưng mọi scene trở nên gần giống nhau hoặc không còn phản ánh đúng story.

# **9. Research Hypotheses**

Các giả thuyết dưới đây là giả thuyết làm việc ban đầu. Chúng phải được điều chỉnh nếu RESEARCH-02 cho thấy failure pattern thực tế khác với dự kiến.

## **H1 — Character-Specific Memory Hypothesis**

> **Việc duy trì identity representation riêng biệt cho từng recurring character sẽ cải thiện long-range identity consistency so với cơ chế chủ yếu phụ thuộc vào recent/global shared information.**

Dự đoán:

Khi character tái xuất hiện sau khoảng cách lớn, phương pháp đề xuất sẽ có identity consistency cao hơn baseline.

Có thể biểu diễn kỳ vọng:

IdentityScoreproposed(d)\>IdentityScorebaseline(d)IdentityScore\_{proposed}(d) \> IdentityScore\_{baseline}(d)

đặc biệt khi dd lớn.

## **H2 — Scene-Aware Retrieval Hypothesis**

> **Chỉ truy xuất identity memory của các character thực sự xuất hiện trong scene sẽ giảm unnecessary identity interaction và giảm identity leakage.**

Nếu:

Ct⊂CC_t \subset C

thì generation của sts_t chỉ sử dụng:

Mt={Mi∣ci∈Ct}M_t=\\M_i \mid c_i\in C_t\\

thay vì toàn bộ:

MM

Dự đoán:

Leakagescene−aware\<Leakageglobal−memoryLeakage\_{scene-aware} \< Leakage\_{global-memory}

## **H3 — Identity Isolation Hypothesis**

> **Tách representation của từng character sẽ cải thiện khả năng phân biệt nhiều recurring characters trong cùng story.**

Một phương pháp thành công phải đạt đồng thời:

High intra−character similarityHigh\\ intra-character\\ similarity

và:

Low inter−character confusionLow\\ inter-character\\ confusion

Nói cách khác:

Ảnh của Aria qua nhiều scene phải giống Aria.

Nhưng Aria không được ngày càng giống Kael.

## **H4 — Identity–Appearance Separation Hypothesis**

> **Tách identity-defining information khỏi scene-dependent appearance information sẽ giúp duy trì identity mà không làm giảm khả năng thay đổi outfit, expression, pose hoặc trạng thái theo scene.**

Phương pháp mới được kỳ vọng đạt sự cân bằng tốt hơn giữa:

Identity PreservationIdentity\\ Preservation

và:

Scene ComplianceScene\\ Compliance

thay vì tối ưu một phía và làm suy giảm phía còn lại.

# **10. Null Hypotheses**

Để tránh thiết kế nghiên cứu chỉ nhằm chứng minh phương pháp đề xuất, các null hypothesis tương ứng cũng phải được chấp nhận như khả năng hợp lệ.

### **H0-1**

Character-specific identity memory không tạo cải thiện đáng kể về long-range identity consistency so với baseline.

### **H0-2**

Scene-aware retrieval không làm giảm identity leakage đáng kể.

### **H0-3**

Identity isolation không cải thiện đáng kể khả năng phân biệt nhiều character.

### **H0-4**

Tách identity và appearance không tạo cải thiện đáng kể trong trade-off giữa character consistency và scene alignment.

Nếu dữ liệu thực nghiệm ủng hộ H0, nghiên cứu phải báo cáo kết quả đó thay vì thay đổi tiêu chí sau khi quan sát kết quả.

# **11. Independent Variables**

Các biến độc lập quan trọng dự kiến gồm:

| **Variable**                   | **Ví dụ**                                           |
|--------------------------------|-----------------------------------------------------|
| Generation Method              | StoryDiffusion / DreamStory / Story-Iter / Proposed |
| Character Gap                  | khoảng cách từ lần xuất hiện trước                  |
| Number of Characters           | 1 / 2 / nhiều character                             |
| Co-occurrence                  | character xuất hiện riêng hay cùng scene            |
| Appearance Variation           | thấp / trung bình / cao                             |
| Identity Memory                | enabled / disabled                                  |
| Scene-Aware Selection          | enabled / disabled                                  |
| Identity–Appearance Separation | enabled / disabled                                  |

Cấu hình chính xác thuộc RESEARCH-04.

# **12. Dependent Variables**

Các outcome cần quan sát gồm bốn nhóm chính.

### **Identity Preservation**

Đo mức độ character vẫn được nhận biết là cùng identity qua nhiều scene.

### **Identity Separation**

Đo mức độ hệ thống tránh nhầm lẫn hoặc pha trộn nhiều character.

### **Scene Alignment**

Đo mức độ generated image phản ánh đúng scene description và appearance state.

### **Image Quality**

Kiểm tra việc tăng consistency có làm suy giảm chất lượng hình ảnh tổng thể hay không.

Metric cụ thể chưa được khóa trong RESEARCH-01.

# **13. Control Variables**

Để comparison có ý nghĩa, experiment sau này phải kiểm soát tối đa các yếu tố không thuộc phương pháp nghiên cứu.

Các cấu hình quan trọng cần được ghi nhận gồm:

- model/checkpoint;

- dataset sample;

- prompt;

- image resolution;

- inference configuration;

- reference images;

- random seed nếu có;

- software/method version;

- generation parameters.

Kiến trúc hệ thống hiện tại cũng yêu cầu lưu model identifier, checkpoint/version, context snapshot, generation parameters, seed và software/method version để hỗ trợ reproducibility.

# **14. Research Population and Benchmark Context**

Benchmark chính dự kiến là **ViStoryBench**.

Tài liệu hiện tại mô tả benchmark gồm:

- 80 story;

- 1.317 scene;

- 344 character;

- 509 character reference images;

- mỗi story từ 4 đến 30 scene.

Benchmark có các trường hợp phù hợp để nghiên cứu recurring character, multiple characters, appearance variation và scene alignment.

RESEARCH-01 không quyết định sử dụng toàn bộ dataset hay subset.

Sampling protocol sẽ được xác định trong RESEARCH-04.

# **15. Baseline Strategy**

Research không bắt đầu bằng Proposed Method.

Quy trình dự kiến:

Literature ReviewLiterature\\ Review ↓\downarrow Baseline ReproductionBaseline\\ Reproduction ↓\downarrow Failure AnalysisFailure\\ Analysis ↓\downarrow Confirmed Research GapConfirmed\\ Research\\ Gap ↓\downarrow Proposed MethodProposed\\ Method ↓\downarrow Controlled ExperimentControlled\\ Experiment ↓\downarrow AblationAblation

StoryDiffusion được dự kiến làm baseline chính; DreamStory và Story-Iter được dùng làm các hướng đối chiếu liên quan tới multi-subject và long-story generation.

# **16. Falsifiability Criteria**

Mỗi hypothesis phải có khả năng bị bác bỏ.

Phương pháp đề xuất không được xem là thành công chỉ vì một số ảnh mẫu nhìn đẹp hơn.

Ví dụ H1 có thể bị bác bỏ nếu:

IdentityScoreproposed≤IdentityScorebaselineIdentityScore\_{proposed} \leq IdentityScore\_{baseline}

trong long-range cases.

H2 có thể bị bác bỏ nếu scene-aware retrieval không làm giảm leakage hoặc thậm chí làm leakage tăng.

H4 có thể bị bác bỏ nếu identity consistency tăng nhưng scene alignment giảm đáng kể.

Do đó conclusion phải dựa trên measurement được định nghĩa trước trong RESEARCH-04.

# **17. Expected Research Contribution**

Nếu các hypothesis chính được thực nghiệm ủng hộ, contribution dự kiến của luận văn có thể bao gồm ba thành phần.

### **C1 — Empirical Contribution**

Một phân tích có hệ thống về failure của các phương pháp hiện tại đối với:

- long-range recurrence;

- multiple recurring characters;

- identity leakage;

- scene-dependent appearance variation.

### **C2 — Methodological Contribution**

Một phương pháp **Scene-Aware Multi-Character Identity Memory** nhằm duy trì identity representation riêng cho recurring characters và chỉ kích hoạt identity information phù hợp với scene.

### **C3 — Evaluation Contribution**

Một protocol đánh giá kết hợp:

- long-range consistency;

- identity separation;

- scene alignment;

- image quality;

- ablation;

- và khi cần, human evaluation.

Contribution cuối cùng chỉ được xác nhận sau thực nghiệm. Tài liệu hiện tại cũng quy định phương pháp đề xuất phải giải quyết một hạn chế quan sát được và đo lường được thay vì chỉ bổ sung một module mà không chứng minh sự cần thiết.

# **18. Out of Scope**

RESEARCH-01 không đặt mục tiêu nghiên cứu:

- video temporal consistency liên tục giữa frame;

- motion generation;

- permanent character identity giữa nhiều project độc lập;

- narrative memory/RAG như contribution khoa học chính;

- full story-writing model;

- page layout hoặc comic editor;

- speech bubble generation;

- collaborative story creation;

- application architecture như contribution nghiên cứu chính.

Các thành phần application có thể hỗ trợ luận văn nhưng không thay thế contribution về visual character consistency.

# **19. Dependency on RESEARCH-02**

Các hypothesis trong tài liệu này là **preliminary hypotheses**.

RESEARCH-02 có nhiệm vụ kiểm tra xem các failure được giả định ở đây có thực sự xuất hiện khi tái hiện baseline hay không.

Ví dụ:

Nếu StoryDiffusion không cho thấy long-range degradation rõ ràng nhưng cho thấy multi-character leakage rất mạnh, trọng tâm phương pháp có thể chuyển nhiều hơn sang identity isolation.

Nếu baseline đã giải quyết tốt identity leakage nhưng yếu với appearance variation, RESEARCH-03 phải ưu tiên identity–appearance decomposition.

Do đó:

> **Research gap cuối cùng phải được xác nhận bằng baseline evidence, không chỉ bằng suy luận từ literature.**

# **20. Dependency on RESEARCH-03**

RESEARCH-03 chỉ được khóa kiến trúc sau khi RESEARCH-02 hoàn thành failure analysis.

RESEARCH-03 phải trả lời ít nhất:

Mi=?M_i = ?

Identity memory được biểu diễn bằng gì?

Write(Mi)=?Write(M_i) = ?

Memory được tạo hoặc cập nhật thế nào?

Retrieve(Mi,st)=?Retrieve(M_i,s_t) = ?

Memory nào được chọn tại scene hiện tại?

Condition(G,Mi,st)=?Condition(G,M_i,s_t) = ?

Identity information được đưa vào generation model bằng cách nào?

Và:

Isolation(Mi,Mj)=?Isolation(M_i,M_j) = ?

làm sao giảm interference giữa hai character khác nhau?

# **21. Dependency on RESEARCH-04**

RESEARCH-04 phải biến mỗi hypothesis thành một experiment có thể chạy.

Ví dụ:

**H1**

Character-specific memory  
→ long-range recurrence experiment.

**H2**

Scene-aware retrieval  
→ leakage experiment.

**H3**

Identity isolation  
→ multi-character experiment.

**H4**

Identity–appearance separation  
→ appearance variation experiment.

Ablation study phải loại từng component để xác định component nào thực sự tạo ra improvement.

# **22. Research Logic Summary**

Toàn bộ logic nghiên cứu của luận văn có thể tóm tắt như sau:

Existing MethodsExisting\\ Methods ↓\downarrow Long Range+Multi Character Failure?Long\\ Range + Multi\\ Character\\ Failure? ↓\downarrow Baseline EvidenceBaseline\\ Evidence ↓\downarrow Character-Specific Identity MemoryCharacter\text{-}Specific\\ Identity\\ Memory ++ Scene-Aware RetrievalScene\text{-}Aware\\ Retrieval ++ Identity/Appearance SeparationIdentity/Appearance\\ Separation ↓\downarrow Improved Identity Preservation?Improved\\ Identity\\ Preservation? ++ Reduced Leakage?Reduced\\ Leakage? ++ Preserved Scene Alignment?Preserved\\ Scene\\ Alignment? ↓\downarrow Controlled ExperimentsControlled\\ Experiments ↓\downarrow Accept/Reject HypothesesAccept / Reject\\ Hypotheses

# **23. Final Research Principle**

Nguyên tắc quan trọng nhất của RESEARCH-01 là:

> **The proposed method is not the starting assumption of the thesis; it is a hypothesis-driven response to failures that must first be demonstrated empirically.**

Nói cách khác:

> Không bắt đầu bằng việc chứng minh rằng Scene-Aware Multi-Character Identity Memory là đúng.

Ta bắt đầu bằng việc xác định:

**Baseline thực sự sai ở đâu?**

Sau đó mới hỏi:

**Thiết kế nào có thể giải quyết failure đó?**

Và cuối cùng:

**Dữ liệu thực nghiệm có chứng minh thiết kế đó hiệu quả hay không?**
