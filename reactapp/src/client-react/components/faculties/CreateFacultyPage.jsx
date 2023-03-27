import UpdateFaculty from "./UpdateFaculty.jsx";
import ModalWindowCreate from "./ModalWindows/ModalWindowCreate.jsx";

import FacultiesApi from "../../api/FacultiesApi.js";

import React from 'react';
import { useNavigate } from 'react-router-dom'

export default function CreateFacultyPage() {
    const [faculty, setFaculty] = React.useState({
        shortName: "",
        fullName: "",
        img: "\\img\\Faculties\\Default.jpg"
    });
    const { shortName, fullName, img } = faculty;
    const [uploadImg, setUploadImg] = React.useState();
    const [errors, setErrors] = React.useState();
    const [modelErrors, setModelErrors] = React.useState();

    const navigate = useNavigate();

    const onChangeModel = (updatedFaculty, updatedUploadImg) => {
        setUploadImg(updatedUploadImg);
        setFaculty(updatedFaculty);
    }

    const onCreateFaculty = () => {
        const form = new FormData();
        form.append("Faculty.FullName", fullName);
        form.append("Faculty.ShortName", shortName);
        form.append("Img", uploadImg);
        var xhr = new XMLHttpRequest();
        xhr.open("post", FacultiesApi.getPostUrl(), true);
        xhr.onload = function () {
            if (xhr.status === 200) {
                navigate('/Faculties');
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
        }.bind(this);
        xhr.send(form);
        $('#facultyCreateModalWindow').modal('hide');
    }

    return (
        <React.Suspense>
            <h1 className="input-group-lg text-center">Создать факультет</h1>
            <hr />
            <ModalWindowCreate onCreate={onCreateFaculty} />
            <div className="row justify-content-center">
                <UpdateFaculty faculty={faculty} errors={errors} modelErrors={modelErrors} onChangeModel={onChangeModel} />
            </div>
            <div className="col text-center pt-4">
                <button type="button" className="btn btn-outline-success btn-lg" data-bs-toggle="modal" data-bs-target="#facultyCreateModalWindow" data-bs-shortname={shortName}>
                    Создать
                </button>
                <a type="button" className="btn btn-outline-secondary btn-lg" href={"/Faculties"}>Вернуться</a>
            </div>
        </React.Suspense>
    );
}