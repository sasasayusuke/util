from fastapi import APIRouter

from app.routers import (
    admin,
    auth,
    customer,
    karten,
    man_hour,
    master_karten,
    master,
    miscellaneous,
    notification,
    project,
    schedule,
    survey,
    survey_master,
    user,
    solver_corporation,
)

router = APIRouter(prefix="/api")
router.include_router(admin.router)
router.include_router(auth.router)
router.include_router(customer.router)
router.include_router(karten.router)
router.include_router(man_hour.router)
router.include_router(master_karten.router)
router.include_router(master.router)
router.include_router(survey.router)
router.include_router(survey_master.router)
router.include_router(project.router)
router.include_router(miscellaneous.router)
router.include_router(notification.router)
router.include_router(project.router)
router.include_router(schedule.router)
router.include_router(survey.router)
router.include_router(survey_master.router)
router.include_router(user.router)
router.include_router(solver_corporation.router)
