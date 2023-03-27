export default class ArchiveApi {
    static baseUrl = "/api/ArchiveApi/";

    static getArchveFormsUrl(year) { return this.baseUrl + "GetArchveFormsByYear/" + year }
    static getYearsUrl() { return this.baseUrl }
    static getArchveByYearAndFormUrl(year, form) { return this.baseUrl + '/GetArchveByYearAndForm/' + year + "/" + form }
}