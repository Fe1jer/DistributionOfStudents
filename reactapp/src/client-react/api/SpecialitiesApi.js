export default class SpecialitiesApi {
    static baseUrl = "/api/SpecialitiesApi/";

    static getFacultySpecialitiesUrl(facultyName) { return this.baseUrl + "FacultySpecialities/" + facultyName }
    static getGroupSpecialitiesUrl(groupId) { return this.baseUrl + "GroupSpecialities/" + groupId }
    static getSpecialityUrl(id) { return this.baseUrl + id }
    static getPostUrl(facultyName) { return this.baseUrl + facultyName }
    static getPutUrl(id) { return this.baseUrl + id }
    static getDeleteUrl(id) { return this.baseUrl + id }
}