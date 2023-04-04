export default class RecruitmentPlansApi {
    static baseUrl = "/api/RecruitmentPlansApi/";

    static getRecruitmentPlansUrl() { return this.baseUrl }
    static getFacultyRecruitmentPlansUrl(facultyShortName, year) { return this.baseUrl + "/" + facultyShortName + "/" + year }
    static getFacultyLastYearRecruitmentPlasUrl(facultyShortName) { return this.baseUrl + "/" + facultyShortName + "/lastYear" }
    static getPostUrl(facultyShortName, year) { return this.baseUrl + '/' + facultyShortName + "/" + year }
    static getPutUrl(facultyShortName, year) { return this.baseUrl + '/' + facultyShortName + "/" + year }
    static getDeleteUrl(facultyShortName, year) { return this.baseUrl + '/' + facultyShortName + "/" + year }
}