from app.routers import (
    auth,
    customer,
    karten,
    man_hour,
    master,
    master_karten,
    miscellaneous,
    notification,
    project,
    schedule,
    solver,
    solver_corporation,
    survey,
    survey_master,
    user,
)

from fastapi import APIRouter

router = APIRouter(prefix="/api")

router.include_router(auth.router)
router.include_router(customer.router)
router.include_router(karten.router)
router.include_router(man_hour.router)
router.include_router(master.router)
router.include_router(master_karten.router)
router.include_router(survey.router)
router.include_router(notification.router)
router.include_router(schedule.router)
router.include_router(solver.router)
router.include_router(solver_corporation.router)
router.include_router(survey_master.router)
router.include_router(user.router)
router.include_router(project.router)
router.include_router(miscellaneous.router)
