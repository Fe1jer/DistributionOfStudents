import React from 'react';

export default function UpdateFaculty({ faculty, modelErrors, errors, onChangeModel }) {
    const [updatedFaculty, setUpdatedFaculty] = React.useState({
        shortName: faculty.shortName,
        fullName: faculty.fullName,
        img: faculty.img
    });
    const { shortName, fullName, img } = updatedFaculty;
    const [uploadImg, setUploadImg] = React.useState(null);
    const [preview, setPreview] = React.useState(faculty.img);
    const onChangeFaculty = (event) => {
        const { name, value } = event.target;
        setUpdatedFaculty((prevState) => {
            return {
                ...prevState,
                [name]: value,
            };
        });
    }
    const showPreview = (e) => {
        const reader = new FileReader();
        reader.onload = x => {
            setPreview(x.target.result);
        }
        reader.readAsDataURL(e.target.files[0]);
        setUploadImg(e.target.files[0]);
    };
    React.useEffect(() => {
        onChangeModel(updatedFaculty, uploadImg);
    }, [updatedFaculty, uploadImg])

    const _formGroupErrors = (errors) => {
        return (
            <span className="text-danger">{
                errors.map((error) =>
                    <React.Suspense>
                        <span>{error}</span>
                        <br></br>
                    </React.Suspense>
                )}
            </span>
        );
    }
    const onSubmit = (e) => {
        e.preventDefault();
    }
    return (
        <form id="formFaculty" onSubmit={onSubmit}>
            <div className="row justify-content-center">
                <div className="col-md-6">
                    <div className=" text-center">
                        {modelErrors ? _formGroupErrors(modelErrors) : ""}
                    </div>
                    <div className="form-group pt-2" style={{ textAlign: "-webkit-center" }}>
                        <img className="scale card-image" src={preview} style={{ objectFit: "cover", width: 290 }} ></img>
                        <input className="form-control" style={{ width: 290 }} type="file" name="Img" accept=".jpg, .jpeg, .png" style={{ width: 290 }} onChange={showPreview} />
                        <span className="text-danger">{errors ? _formGroupErrors(errors.Img) : ""}</span>
                    </div>
                    <div className="form-group pt-3">
                        <label className="control-label">Полное название</label><sup>*</sup>
                        <input className="form-control" name="fullName" onChange={onChangeFaculty} value={fullName} />
                        <span className="text-danger">{errors ? _formGroupErrors(errors["Faculty.FullName"]) : ""}</span>
                    </div >
                    <div className="form-group pt-3">
                        <label className="control-label"></label>Сокращённое название<sup>*</sup>
                        <input className="form-control" name="shortName" onChange={onChangeFaculty} value={shortName} />
                        <span className="text-danger">{errors ? _formGroupErrors(errors["Faculty.ShortName"]) : ""}</span>
                    </div >
                </div>
            </div >
        </form >
    );
}