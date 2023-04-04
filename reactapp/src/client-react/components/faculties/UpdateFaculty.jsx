import React from 'react';
import Form from 'react-bootstrap/Form';
import Image from 'react-bootstrap/Image'

export default function UpdateFaculty({ faculty, modelErrors, errors, onChangeModel }) {
    const [updatedFaculty, setUpdatedFaculty] = React.useState({
        id: faculty.id,
        shortName: faculty.shortName,
        fullName: faculty.fullName,
        img: faculty.img
    });
    const { id, shortName, fullName, img } = updatedFaculty;
    const [uploadImg, setUploadImg] = React.useState(null);
    const [preview, setPreview] = React.useState(img);
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
        if (errors) {
            return (<React.Suspense>{
                errors.map((error) =>
                    <React.Suspense key={error}><span>{error}</span><br></br></React.Suspense>
                )}
            </React.Suspense>);
        }
    }
    return (
        <React.Suspense>
            <Form.Group style={{ textAlign: "-webkit-center" }}>
                <Form.Control className="p-0 d-none" plaintext readOnly isInvalid={modelErrors ? !!modelErrors : false} />
                <Form.Control.Feedback type="invalid">{modelErrors ? _formGroupErrors(modelErrors) : ""}</Form.Control.Feedback>               
            </Form.Group>
            <Form.Group style={{ textAlign: "-webkit-center" }}>
                <Image className="scale card-image m-0 text-center" src={preview} style={{ objectFit: "cover", width: 290 }}></Image>
                <Form.Control type="file" accept=".jpg, .jpeg, .png" style={{ width: 290 }}
                    name="Img" onChange={showPreview}
                    isInvalid={errors ? !!errors.Img : false} />
                <Form.Control.Feedback className="m-0" type="invalid">{errors ? _formGroupErrors(errors.Img) : ""}</Form.Control.Feedback>
            </Form.Group>
            <Form.Group className="pt-3">
                <Form.Label className="mb-0">Полное название</Form.Label><sup>*</sup>
                <Form.Control type="text"
                    required name="fullName" value={fullName ?? ""} onChange={onChangeFaculty}
                    isInvalid={errors ? !!errors["Faculty.FullName"] : false} />
                <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors["Faculty.FullName"]) : ""}</Form.Control.Feedback>
            </Form.Group>
            <Form.Group className="pt-3">
                <Form.Label className="mb-0">Полное название</Form.Label><sup>*</sup>
                <Form.Control type="text"
                    required name="shortName" value={shortName ?? ""} onChange={onChangeFaculty}
                    isInvalid={errors ? !!errors["Faculty.ShortName"] : false} />
                <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors["Faculty.ShortName"]) : ""}</Form.Control.Feedback>
            </Form.Group>
        </React.Suspense>
    );
}