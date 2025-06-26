from typing import List
from app.schemas.base import CustomBaseModel


class UpdateCurrentProgram(CustomBaseModel):
    version: int
    project_id: str
    is_current: bool = None
    customer_success_result: str = None
    customer_success_result_factor: str = None
    next_support_content: str = None
    support_issue: str = None
    support_success_factor: str = None
    survey_customer_self_assessment: str = None
    survey_ssap_assessment: str = None


class UpdateNextProgram(CustomBaseModel):
    version: int
    project_id: str
    is_current: bool
    is_publishable: bool
    president_policy: str
    kpi: str
    three_years_to_be: str
    present_state: str
    issue: str
    request: str
    customer_success: str
    customer_success_reuse: bool
    schedule: str
    lineup: str
    support_contents: str
    need_personal_skill: str
    need_partner: str
    supplement_human_resource_to_sap: str
    current_customer_profile: str
    want_acquire_customer_profile: str
    our_strengths: str
    aspiration: str
    mission: str
    persons: str
    manager: str
    commercialization_skill: str
    exist_partners: str
    support_order: str
    exist_evaluation: str
    exist_audition: str
    exist_ideation: str
    exist_idea_review: str
    budget: str
    human_resource: str
    idea: str
    theme: str
    client: str
    client_issue: str
    solution: str
    originality: str
    mvp: str
    tam: str
    sam: str
    is_right_time: str
    load_map: str


class CreateCurrentProgram(CustomBaseModel):
    is_current: bool
    customer_success_result: str = None
    customer_success_result_factor: str = None
    next_support_content: str = None
    support_issue: str = None
    support_success_factor: str = None
    survey_customer_self_assessment: str = None
    survey_ssap_assessment: str = None


class CreateNextProgram(CustomBaseModel):
    project_id: str
    is_current: bool
    is_publishable: bool
    president_policy: str
    kpi: str
    three_years_to_be: str
    present_state: str
    issue: str
    request: str
    customer_success: str
    customer_success_reuse: bool
    schedule: str
    lineup: str
    support_contents: str
    need_personal_skill: str
    need_partner: str
    supplement_human_resource_to_sap: str
    current_customer_profile: str
    want_acquire_customer_profile: str
    our_strengths: str
    aspiration: str
    mission: str
    persons: str
    manager: str
    commercialization_skill: str
    exist_partners: str
    support_order: str
    exist_evaluation: str
    exist_audition: str
    exist_ideation: str
    exist_idea_review: str
    budget: str
    human_resource: str
    idea: str
    theme: str
    client: str
    client_issue: str
    solution: str
    originality: str
    mvp: str
    tam: str
    sam: str
    is_right_time: str
    load_map: str
