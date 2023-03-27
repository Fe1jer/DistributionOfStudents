export default class FacultiesApi {
    static baseUrl = "/api/FacultiesApi/";

    static getFacultiesUrl() { return this.baseUrl }
    static getFacultyUrl(shortName) { return this.baseUrl + shortName }
    static getPostUrl() { return this.baseUrl }
    static getPutUrl(shortName) { return this.baseUrl + shortName }
    static getDeleteUrl(shortName) { return this.baseUrl + shortName }
}