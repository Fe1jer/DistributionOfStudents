import UpdateFaculty from "./UpdateFaculty.jsx";
import ModalWindowDelete from "./ModalWindows/ModalWindowDelete.jsx";
import ModalWindowEdit from "./ModalWindows/ModalWindowEdit.jsx";

function CreateFacultyPage({ apiUrl, facultyShortName }) {
    const [faculty, setFaculty] = React.useState({
        shortName: "",
        fullName: "",
        img: ""
    });
    const { shortName, fullName, img } = faculty;
    const [uploadImg, setUploadImg] = React.useState(null);
    const [errors, setErrors] = React.useState();
    const [modelErrors, setModelErrors] = React.useState();

    const onLoad = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", apiUrl + "/" + facultyShortName, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setFaculty(data);
        }.bind(this);
        xhr.send();
    }
    const onChangeModel = (updatedFaculty, updatedUploadImg) => {
        setUploadImg(updatedUploadImg);
        setFaculty(updatedFaculty);
    }
    const onDeleteFaculty = (facultyShortName) => {
        var xhr = new XMLHttpRequest();
        xhr.open("delete", apiUrl + '/' + facultyShortName, true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            if (xhr.status === 200) {
                document.location.pathname = "/Faculties";
            }
        }.bind(this);
        xhr.send();
        $('#facultyDeleteModalWindow').modal('hide');
    }
    const onEditFaculty = () => {
        const form = new FormData();
        form.append("Faculty.FullName", shortName);
        form.append("Faculty.ShortName", fullName);
        form.append("Faculty.Img", img);
        form.append("Img", uploadImg);
        var xhr = new XMLHttpRequest();
        xhr.open("put", apiUrl + "/" + facultyShortName, true);
        xhr.onload = function () {
            if (xhr.status === 200) {
                document.location.pathname = "/Faculties";
            }
            else if (xhr.status === 400) {
                var a = eval('({obj:[' + xhr.response + ']})');
                if (a.obj[0].errors) {
                    setErrors(a.obj[0].errors);
                }
                if (a.obj[0].modelErrors) {
                    setModelErrors(a.obj[0].modelErrors);
                }
            }
        }.bind(form);
        xhr.send(form);
        $('#facultyEditModalWindow').modal('hide');
    }

    React.useEffect(() => {
        onLoad();
    }, [])
    return (
        <React.Suspense>
            <h1 className="input-group-lg text-center">Редактировать факультет</h1>
            <h4 className="text-center">{facultyShortName}</h4>
            <hr />
            <ModalWindowDelete onDelete={onDeleteFaculty} />
            <ModalWindowEdit onEdit={onEditFaculty} />
            <div className="row justify-content-center">
                <UpdateFaculty key={faculty.id} faculty={faculty} errors={errors} modelErrors={modelErrors} onChangeModel={onChangeModel} />
            </div>
            <div className="col text-center pt-4">
                <button type="button" className="btn btn-outline-success btn-lg" data-bs-toggle="modal" data-bs-target="#facultyEditModalWindow" data-bs-shortname={facultyShortName}>
                    Сохранить
                </button>
                <button type="button" className="btn btn-outline-danger btn-lg" data-bs-toggle="modal" data-bs-target="#facultyDeleteModalWindow" data-bs-shortname={facultyShortName} data-bs-fullname={fullName}>
                    Удалить
                </button>
                <a type="button" className="btn btn-outline-secondary btn-lg" href={"/Faculties"}>Вернуться</a>
            </div>
        </React.Suspense>
    );
}

const container = document.getElementById('content');
const root = ReactDOM.createRoot(container);
root.render(<CreateFacultyPage apiUrl="/api/FacultiesApi" facultyShortName="1" />);