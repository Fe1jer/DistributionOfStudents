export default class GroupsOfSpecialitiesApi {
    static baseUrl = "/api/GroupsOfSpecialtiesApi/";

    static getFacultyGroupsUrl(facultyShortName, year) { return this.baseUrl + "FacultyGroups/" + facultyShortName + "/" + year }
    static getGroupUrl(id) { return this.baseUrl + id }
    static getPostUrl(facultyShortName) { return this.baseUrl + facultyShortName }
    static getPutUrl(facultyShortName) { return this.baseUrl + facultyShortName }
    static getDeleteUrl(facultyShortName, id) { return this.baseUrl + facultyShortName + "/" + id }
}