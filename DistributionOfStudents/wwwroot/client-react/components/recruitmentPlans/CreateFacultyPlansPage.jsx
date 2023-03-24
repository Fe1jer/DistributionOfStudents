import UpdateSpecialityPlansList from "./UpdateSpecialityPlansList.jsx";
import ModalWindowCreate from "./ModalWindows/ModalWindowCreate.jsx";

function CreateFacultyPlansPage({ apiUrl, facultyShortName, lastYear }) {
    const [plans, setPlans] = React.useState([]);
    const [year, setYear] = React.useState(lastYear + 1);
    const [facultyName, setFacultyName] = React.useState("");
    const [errors, setErrors] = React.useState({});

    const loadData = () => {
        if (year == 1) {
            var now = new Date();
            setYear(now.getFullYear());
        }
        var xhr = new XMLHttpRequest();
        xhr.open("get", apiUrl + "/" + facultyShortName + "?year=" + year, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setPlans(data.plansForSpecialities);
            setFacultyName(data.facultyFullName);
        }.bind(this);
        xhr.send();
    }
    const onCangeYear = (e) => {
        setYear(e.target.value);
    }
    const onChangePlans = (changedPlans) => {
        setPlans(changedPlans);
    }

    const onCreateFacultyPlans = () => {
        if (year != 1) {
            var xhr = new XMLHttpRequest();
            xhr.open("post", apiUrl + '/' + facultyShortName + "?year=" + year, true);
            xhr.setRequestHeader("Content-Type", "application/json")
            xhr.onload = function () {
                if (xhr.status === 200) {
                    $('#facultyPlansCreateModalWindow').modal('hide');
                    document.location.pathname = "/Faculties/" + facultyShortName;
                }
                else if (xhr.status === 400) {
                    var a = eval('({obj:[' + xhr.response + ']})');
                    setErrors(a.obj[0].errors);
                }
            }.bind(this);
            xhr.send(JSON.stringify(plans));
        }
        else {
            $('#facultyPlansCreateModalWindow').modal('hide');
        }
    }
    React.useEffect(() => {
        loadData();
    }, [year])

    return (
        <React.Suspense>
            <h1 className="input-group-lg text-center">План приёма на <input min="0" type="number" onChange={onCangeYear} className="form-control d-inline" value={year} style={{ width: 100 }} /> год</h1>
            <hr />
            <ModalWindowCreate onCreate={onCreateFacultyPlans} />
            <div className="ps-lg-4 pe-lg-4 position-relative">
                <h4>{facultyName}</h4>
                <UpdateSpecialityPlansList year={year} plans={plans} errors={errors} onChange={onChangePlans} />
            </div>
            <div className="col text-center pt-4">
                <button type="button" className="btn btn-outline-success btn-lg" data-bs-toggle="modal" data-bs-target="#facultyPlansCreateModalWindow" data-bs-year={year} data-bs-facultyshortname={facultyShortName}>
                    Создать
                </button>
                <a type="button" className="btn btn-outline-secondary btn-lg" href={"/Faculties/" + facultyShortName}>В факультет</a>
            </div>
        </React.Suspense>
    );
}

const facultyShortName = window.location.pathname.split('/')[2];

const container = document.getElementById('content');
const root = ReactDOM.createRoot(container);
root.render(<CreateFacultyPlansPage apiUrl="/api/RecruitmentPlansApi" facultyShortName={facultyShortName} lastYear={0} />);