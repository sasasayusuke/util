import {
  addBusinessDays,
  subBusinessDays,
  differenceInCalendarDays,
  addDays,
} from 'date-fns'

export function addBusinessDaysToNextMonthStart(date: Date, day: number) {
  const nextMonthStart = new Date(date.getFullYear(), date.getMonth() + 1, 1)
  return addBusinessDays(nextMonthStart, day)
}

export function countBusinessDayBeyondAMonth(from: number, to: number): number {
  let holyday = 0

  const now: Date = new Date()
  const fromDay: Date = new Date(now.getFullYear(), now.getMonth(), from)
  const toDay: Date = new Date(now.getFullYear(), now.getMonth() + 1, to)
  const difference: number = differenceInCalendarDays(fromDay, toDay)

  // 余裕があれば、7日以上の場合は日数を2/7にして計算量を減らす
  for (let i = 0; i < difference; i++) {
    const targetDay = addDays(fromDay, i)
    if ([0, 6].includes(targetDay.getDay())) {
      holyday += 1
    }
  }

  return difference - holyday
}
