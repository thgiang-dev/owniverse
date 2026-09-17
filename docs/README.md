# Documentation inventory

`/docs` is the source of truth for approved OWNIVERSE specifications.

**Status: imported 31 approved specification documents and one image from the user-provided `OWNIVERSE-docs-markdown.zip`.** Specification and asset contents are preserved byte-for-byte. This inventory is a setup aid, not a substitute specification.

## Package provenance

Extracted from `Character-Consistent Long-Range Story Generation.docx`.

## Imported specifications

Paths below are relative to `/docs`.

| Specification | File |
| --- | --- |
| PROD-01 | [product/PROD-01_Product_Requirements_Specification.md](product/PROD-01_Product_Requirements_Specification.md) |
| PROD-02 | [product/PROD-02_Functional_and_Workflow_Specification.md](product/PROD-02_Functional_and_Workflow_Specification.md) |
| UX-01 | [ux/UX-01_Information_Architecture_and_UX_Flow_Specification.md](ux/UX-01_Information_Architecture_and_UX_Flow_Specification.md) |
| UX-02 | [ux/UX-02_UI_Design_System_and_Interaction_Specification.md](ux/UX-02_UI_Design_System_and_Interaction_Specification.md) |
| UX-03 | [ux/UX-03_Visual_Direction_and_Reference_Screen_Specification.md](ux/UX-03_Visual_Direction_and_Reference_Screen_Specification.md) |
| ARCH-01 | [architecture/ARCH-01_Solution_Architecture_Specification.md](architecture/ARCH-01_Solution_Architecture_Specification.md) |
| ARCH-02 | [architecture/ARCH-02_Runtime_Job_Orchestration_and_Deployment_Specification.md](architecture/ARCH-02_Runtime_Job_Orchestration_and_Deployment_Specification.md) |
| DOMAIN-01 | [domain/DOMAIN-01_Project_and_Story_Core_Domain_Specification.md](domain/DOMAIN-01_Project_and_Story_Core_Domain_Specification.md) |
| DOMAIN-02 | [domain/DOMAIN-02_Story_State_Timeline_Canon_and_Versioning_Specification.md](domain/DOMAIN-02_Story_State_Timeline_Canon_and_Versioning_Specification.md) |
| AI-01 | [ai/AI-01_AI_Architecture_and_Orchestration_Specification.md](ai/AI-01_AI_Architecture_and_Orchestration_Specification.md) |
| AI-02 | [ai/AI-02_Context_Memory_and_Consistency_Specification.md](ai/AI-02_Context_Memory_and_Consistency_Specification.md) |
| AI-03 | [ai/AI-03_Generation_Validation_and_Output_Specification.md](ai/AI-03_Generation_Validation_and_Output_Specification.md) |
| DATA-01 | [data/DATA-01_Data_Model_and_Persistence_Specification.md](data/DATA-01_Data_Model_and_Persistence_Specification.md) |
| DATA-02 | [data/DATA-02_Asset_Versioning_and_Storage_Specification.md](data/DATA-02_Asset_Versioning_and_Storage_Specification.md) |
| API-01 | [api/API-01_API_Contract_and_Integration_Specification.md](api/API-01_API_Contract_and_Integration_Specification.md) |
| API-02 | [api/API-02_Async_Job_Event_and_Error_Contract_Specification.md](api/API-02_Async_Job_Event_and_Error_Contract_Specification.md) |
| RESEARCH-01 | [research/RESEARCH-01_Research_Problem_and_Hypotheses_Specification.md](research/RESEARCH-01_Research_Problem_and_Hypotheses_Specification.md) |
| RESEARCH-02 | [research/RESEARCH-02_Baseline_Reproduction_and_Failure_Analysis_Protocol.md](research/RESEARCH-02_Baseline_Reproduction_and_Failure_Analysis_Protocol.md) |
| RESEARCH-03 | [research/RESEARCH-03_Scene_Aware_Multi_Character_Identity_Memory_Method_Specification.md](research/RESEARCH-03_Scene_Aware_Multi_Character_Identity_Memory_Method_Specification.md) |
| RESEARCH-04 | [research/RESEARCH-04_Experiment_Metrics_and_Ablation_Protocol.md](research/RESEARCH-04_Experiment_Metrics_and_Ablation_Protocol.md) |
| QA-01 | [qa/QA-01_Master_Test_Plan_and_Quality_Strategy.md](qa/QA-01_Master_Test_Plan_and_Quality_Strategy.md) |
| QA-02 | [qa/QA-02_AI_Image_and_Consistency_Evaluation_Specification.md](qa/QA-02_AI_Image_and_Consistency_Evaluation_Specification.md) |
| QA-03 | [qa/QA-03_Functional_and_Integration_Test_Case_Specification.md](qa/QA-03_Functional_and_Integration_Test_Case_Specification.md) |
| QA-04 | [qa/QA-04_User_Acceptance_and_Release_Readiness_Specification.md](qa/QA-04_User_Acceptance_and_Release_Readiness_Specification.md) |
| DEPLOY-01 | [deployment/DEPLOY-01_Local_Development_and_Environment_Configuration.md](deployment/DEPLOY-01_Local_Development_and_Environment_Configuration.md) |
| DEPLOY-02 | [deployment/DEPLOY-02_Containerization_and_Runtime_Orchestration_Specification.md](deployment/DEPLOY-02_Containerization_and_Runtime_Orchestration_Specification.md) |
| DEPLOY-03 | [deployment/DEPLOY-03_Deployment_Operations_Monitoring_and_Backup.md](deployment/DEPLOY-03_Deployment_Operations_Monitoring_and_Backup.md) |
| IMPLEMENT-01 | [implementation/IMPLEMENT-01_MVP_Implementation_Roadmap.md](implementation/IMPLEMENT-01_MVP_Implementation_Roadmap.md) |
| DEV-01 | [implementation/DEV-01_Repository_Coding_and_Engineering_Convention.md](implementation/DEV-01_Repository_Coding_and_Engineering_Convention.md) |
| IMPLEMENT-02 | [implementation/IMPLEMENT-02_Module_Implementation_and_Dependency_Plan.md](implementation/IMPLEMENT-02_Module_Implementation_and_Dependency_Plan.md) |
| SPRINT-01 | [implementation/SPRINT-01_MVP_Sprint_Execution_Plan.md](implementation/SPRINT-01_MVP_Sprint_Execution_Plan.md) |

## Assets

- [assets/image1.png](assets/image1.png)

## Import verification

- All 31 documents listed in the package manifest are present; all specification groups expected by this workspace are represented.
- No duplicate archive paths, duplicate specification identifiers, or identical-content files were found.
- The package `README.md` conflicted with this existing index. Its provenance and inventory were incorporated here while preserving the existing source-of-truth guidance and import procedure.
- The package `README.md` and `manifest.json` were not copied as separate files; they contain packaging/index metadata.
- All 32 imported files were verified against the ZIP using SHA-256.
- Specification contents and version choices were not edited or reconciled during this import.
- This import does not create the documentation Git checkpoint or begin Sprint 0.

## Import procedure

1. Obtain the approved, updated source documents from the user. Preserve their contents and identifiers; do not infer missing specifications from filenames or conversation summaries.
2. Copy the originals into `/docs`, keeping meaningful filenames and relative links. Do not delete or move the source originals.
3. Update this inventory with each document's actual relative path and version/date where specified. Ask the user to resolve duplicate or conflicting versions.
4. Verify DEV-01 and SPRINT-01 define the rules and current scope, and verify the full required document set is present.
5. Review the Git diff and commit documentation checkpoint 0 before application scaffolding.
