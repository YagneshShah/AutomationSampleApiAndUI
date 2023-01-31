/* Note:
 * 1. Files(like Precondition, Actions, Assertions, etc) under ./stepdef/app/* directory are meant for steps related to app/project we are working on.
 * 2. Files(like Precondition, Actions, Assertions, etc) under ./stepdef/lib/* directory are meant for common steps libraries which can be used across multiple apps/projects. 
 *    No code which is related to project should be in here.
 */

import { format, addDays, subDays } from 'date-fns';

/** This is a description of the getDate function.
 *
 * WARNING: This function will throw if expectedDay is in the wrong format.
 *          It should not be used for feature code, but should be fine for our
 *          tests because it will fail loudly and early if used incorrectly.
 *
 * @param expectedDay Don't want to hard code the date when we Mock any api or any value, as that is a bad practise. 
 * Hard-coded date can result in automation failure due to business logic.
 * Generic ex:  copy-right year value 2022 may change every year. Hence we should not hard code any date values
 * 
 * possible values: "today" | "tomorrow" | "yesterday" | "today-N" | "today+N".
 * Ex: "today" | "tomorrow" | "yesterday" | "today-30" | "today+10"
 * Today-30: will subtract 30 days from today's date
 * Today+10: will add 10 days to today's date
 * 
 * @param expectedFormat returns the date in expected format you pass
 *  If no value passed then by default returns the date in format 'yyyy-MM-dd'
 *  possible values: "yyyy-MM-dd", "dd MMM yyyy"
 * 
 * @returns a date string in expectedFormat
 */
export function getDate(expectedDay: string, expectedFormat: string = "yyyy-MM-dd"): string {
  const today: Date = new Date();
  if (expectedDay.includes("today-")) {
    const diff: number = Number(expectedDay.split("-")[1]);
    if (isNaN(diff))
      throw new Error(`days passed '${diff}' in expectedDay param '${expectedDay}' is Not a Number...`)
    return format(subDays(today, diff), expectedFormat);
  }
  if (expectedDay.includes("today+")) {
    const diff: number = Number(expectedDay.split("+")[1]);
    return format(addDays(today, diff), expectedFormat);
  }
  if (expectedDay === "today")
    return format(today, expectedFormat);

  if (expectedDay === "yesterday")
    return format(subDays(today, 1), expectedFormat);

  if (expectedDay === "tomorrow")
    return format(addDays(today, 1), expectedFormat);

  throw new Error(`No matching if condition passed for expectedDay: ${expectedDay}`)
}

// Should be appropriate for generating unique IDs in test cases.
export const uniqueID = () => Math.floor(Math.random() * 2000000000)
