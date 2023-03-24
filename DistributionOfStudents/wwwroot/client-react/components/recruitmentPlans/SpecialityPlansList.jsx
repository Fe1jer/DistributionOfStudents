import SpecialityPlan from './SpecialityPlan.jsx';

export default function SpecialityPlansList({ facultyShortName, year, plans }) {
    return <div className="shadow">
        <table className="table table-bordered mb-0">
            <thead>
                <tr>
                    <th rowSpan="2">Специальность (направление специальности)</th>
                    <th colSpan="4">Дневная форма получения образования</th>
                    <th colSpan="4">Заочная форма получения образования</th>
                    <th rowSpan="2" width="60" className="text-center align-middle">
                        <a type="button" className="btn btn-outline-success py-1" href={"/Faculties/" + facultyShortName + "/RecruitmentPlans/Edit?year=" + year}>
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-pencil-square" viewBox="0 0 16 16">
                                <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                                <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" />
                            </svg>
                        </a>
                        <button type="button" className="btn btn-outline-danger py-1" data-bs-toggle="modal" data-bs-target="#facultyPlansDeleteModalWindow" data-bs-year={year} data-bs-facultyshortname={facultyShortName}>
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash3-fill" viewBox="0 0 16 16">
                                <path d="M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5Zm-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5ZM4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06Zm6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528ZM8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5Z" />
                            </svg>
                        </button>
                    </th>
                </tr>
                <tr>
                    <th width="85">Бюджет</th>
                    <th width="85">Платное</th>
                    <th width="85">Бюджет сокр.срок</th>
                    <th width="85">Платное сокр.срок</th>
                    <th width="85">Бюджет</th>
                    <th width="85">Платное</th>
                    <th width="85">Бюджет сокр.срок</th>
                    <th width="85">Платное сокр.срок</th>
                </tr>
            </thead>
            <tbody>{
                plans.map((item) =>
                    <SpecialityPlan key={item.specialityName} specialityPlan={item} />
                )}
            </tbody>
        </table>
    </div>;
}