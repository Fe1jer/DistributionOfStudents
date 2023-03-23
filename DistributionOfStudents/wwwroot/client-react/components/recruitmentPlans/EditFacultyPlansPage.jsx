import UpdateSpecialityPlansList from "./UpdateSpecialityPlansList.jsx";
import ModalWindowEdit from "./ModalWindowEdit.jsx";

function StudentsPage({ apiUrl, facultyShortName, year }) {
    const [plans, setPlans] = React.useState([]);
    const [facultyName, setFacultyName] = React.useState("");
    const [errors, setErrors] = React.useState({});

    const loadData = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", apiUrl + "/" + facultyShortName + "?year=" + year, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setPlans(data.plansForSpecialities);
            setFacultyName(data.facultyFullName);
        }.bind(this);
        xhr.send();
    }
    const onChangePlans = (changedPlans) => {
        setPlans(changedPlans);
    }

    const onEditFacultyPlans = (e) => {
        if (year != 1) {
            var xhr = new XMLHttpRequest();
            xhr.open("put", apiUrl + '/' + facultyShortName + "?year=" + year, true);
            xhr.setRequestHeader("Content-Type", "application/json")
            xhr.onload = function () {
                if (xhr.status === 200) {
                    $('#facultyPlansEditModalWindow').modal('hide');
                    document.location.pathname = "/Faculties/" + facultyShortName;
                    e.preventDefault();
                }
                else if (xhr.status === 400) {
                    var a = eval('({obj:[' + xhr.response + ']})');
                    setErrors(a.obj[0].errors);
                    console.log(xhr.response);
                }
            }.bind(this);
            xhr.send(JSON.stringify(plans));
        }
        else {
            $('#facultyPlansEditModalWindow').modal('hide');
        }
    }
    React.useEffect(() => {
        loadData();
    }, [])

    return (
        <React.Suspense>
            <h1 className="input-group-lg text-center">План приёма на {year} год</h1>
            <hr />
            <ModalWindowEdit onEdit={onEditFacultyPlans} />
            <div className="ps-lg-4 pe-lg-4 position-relative">
                <h4>{facultyName}</h4>
                <UpdateSpecialityPlansList year={year} plans={plans} errors={errors} onChange={onChangePlans} />
            </div>
            <div className="col text-center pt-4">
                <button type="button" className="btn btn-outline-success btn-lg" data-bs-toggle="modal" data-bs-target="#facultyPlansEditModalWindow" data-bs-year={year} data-bs-facultyshortname={facultyShortName}>
                    Сохранить
                </button>
                <a type="button" className="btn btn-outline-secondary btn-lg" href={"/Faculties/" + facultyShortName}>В факультет</a>
            </div>
        </React.Suspense>
    );
}

const facultyShortName = window.location.pathname.split('/')[2];

const container = document.getElementById('content');
const root = ReactDOM.createRoot(container);
root.render(<StudentsPage apiUrl="/api/RecruitmentPlansApi" facultyShortName={facultyShortName} year={2023} />);