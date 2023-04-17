export default class AdmissionsApi {
    static baseUrl = "/api/AdmissionsApi/";

    static getGroupAdmissionsUrl(groupId, searhText, currentPage, pageLimit) { return this.baseUrl + "GroupAdmissions/" + groupId + "?searchStudents=" + searhText + "&page=" + currentPage + "&pageLimit=" + pageLimit }
    static getAdmissionUrl(id) { return this.baseUrl + id }
    static getPostUrl(groupId) { return this.baseUrl + groupId }
    static getPutUrl(id) { return this.baseUrl + id }
    static getDeleteUrl(id) { return this.baseUrl + id }
}