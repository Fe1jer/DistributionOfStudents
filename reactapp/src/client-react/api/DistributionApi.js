export default class DistributionApi {
    static baseUrl = "/api/DistributionApi/";

    static getGroupCompetitionUrl(facultyName, groupId) { return this.baseUrl + facultyName + "/" + groupId + "/Competition" }
}