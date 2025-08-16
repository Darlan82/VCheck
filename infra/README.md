infra (Terraform)

Purpose: Terraform code to provision Azure resources for VCheck.

Structure:
- modules/ -> reusable Terraform modules (app service, sql, static web app, apim)
- environments/ -> tfvars per environment (dev, prod)
- scripts/ -> helper scripts for running terraform

Next steps:
- Initialize Terraform with backend (remote state), create modules, and add basic main.tf resources.
- Add an `azure.tf` provider file and example `backend.tf` for remote state.
