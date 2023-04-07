export default class SubjectsApi {
    static baseUrl = "/api/SubjectsApi/";

    static getSubjectsUrl() { return this.baseUrl }
    static getGroupSubjectsUrl(groupId) { return this.baseUrl + "GroupSubjects/" + groupId }
    static getSubjectUrl(id) { return this.baseUrl + id }
    static getPostUrl() { return this.baseUrl }
    static getPutUrl(id) { return this.baseUrl + id }
    static getDeleteUrl(id) { return this.baseUrl + id }
} 