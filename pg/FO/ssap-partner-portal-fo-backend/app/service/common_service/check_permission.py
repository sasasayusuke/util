def is_assigned_to_project(user_project_ids: set, project_id: str) -> bool:
    """担当案件かどうかを判定します

    Args:
        user_project_ids (set): ユーザーが担当している案件のID
        project_id (str): 案件ID

    Returns:
        bool: 担当している場合はTrue, そうでない場合はFalse
    """
    if user_project_ids is None:
        return False

    return project_id in user_project_ids


def is_project_within_department(
    user_supporter_organization_id: str, project_supporter_organization_id: str
) -> bool:
    """所属している課の案件かどうかを判定します

    Args:
        user_supporter_organization_id (str): 支援者組織ID
        project_supporter_organization_id (str): 粗利メイン課

        bool: 所属している課の案件の場合はTrue, そうでない場合はFalse
    """
    if user_supporter_organization_id is None:
        return False

    return project_supporter_organization_id in user_supporter_organization_id
