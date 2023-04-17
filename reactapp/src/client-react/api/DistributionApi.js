export default class DistributionApi {
    static baseUrl = "/api/DistributionApi/";

    static getDistributionUrl(facultyName, groupId) { return this.baseUrl + facultyName + "/" + groupId }
    static getPostCreateUrl(facultyName, groupId) { return this.baseUrl + facultyName + "/" + groupId + "/CreateDistribution" }
    static getPostConfirmUrl(facultyName, groupId) { return this.baseUrl + facultyName + "/" + groupId + "/ConfirmDistribution" }
    static getDeleteUrl(facultyName, groupId) { return this.baseUrl + facultyName + "/" + groupId }
    static getGroupCompetitionUrl(facultyName, groupId) { return this.baseUrl + facultyName + "/" + groupId + "/Competition" }
}