export default class StatisticApi {
    static baseUrl = "/api/StatisticApi/";

    static getPlansStatisticUrl(facultyName, groupId) { return this.baseUrl + "PlansStatisticChart?groupId=" + groupId + "&facultyName=" + facultyName }
    static getGroupStatisticUrl(groupId) { return this.baseUrl + "GroupStatisticChart?groupId=" + groupId }
    static getPutGroupStatisticUrl(facultyName, groupId) { return this.baseUrl + facultyName + "/" + groupId }
}