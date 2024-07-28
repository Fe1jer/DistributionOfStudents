import { CreateUserValidationSchema } from '../../../validations/User.validation';
import UsersService from "../../../services/Users.service";

import Button from 'react-bootstrap/Button';
import Col from 'react-bootstrap/Col';
import Form from 'react-bootstrap/Form';
import Image from 'react-bootstrap/Image';
import Modal from 'react-bootstrap/Modal';
import Row from 'react-bootstrap/Row';

import { Field } from 'formik';
import * as formik from 'formik';

import React from 'react';

export default function ModalWindowCreate({ show, handleClose, onLoadUsers }) {
    const { Formik } = formik;
    const [preview, setPreview] = React.useState("/img/Users/bntu.jpg");
    const defaultUser = {
        userName: null,
        password: null,
        name: null,
        surname: null,
        patronymic: null
    }

    const handleSubmit = (values) => {
        onCreateUser(values);
    }

    const onCreateUser = async (updatedUser) => {
        const form = new FormData();
        form.append("UserName", updatedUser.userName);
        form.append("Password", updatedUser.password);
        form.append("Name", updatedUser.name);
        form.append("Surname", updatedUser.surname);
        form.append("Patronymic", updatedUser.patronymic);
        form.append("FileImg", updatedUser.fileImg);

        await UsersService.httpPost(form);
        handleClose();
        onLoadUsers();
    }

    const showPreview = (e, form) => {
        const reader = new FileReader();
        reader.onload = x => {
            setPreview(x.target.result);
        }
        reader.readAsDataURL(e.target.files[0]);
        form.setFieldValue("fileImg", e.currentTarget.files[0]);
    };

    return (
        <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
            <Formik
                validationSchema={CreateUserValidationSchema}
                onSubmit={handleSubmit}
                initialValues={{ ...defaultUser, fileImg: null }}>
                {({ handleSubmit, handleChange, values, touched, errors }) => (
                    <Form noValidate onSubmit={handleSubmit}>
                        <Modal.Header closeButton>
                            <Modal.Title as="h5">Добавить пользователя</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <Form.Group style={{ textAlign: "-webkit-center" }}>
                                <Image className="scale card-image m-0 text-center" src={preview} style={{ objectFit: "cover", width: 290 }}></Image>
                                <Field name="fileImg">
                                    {({ form }) => (
                                        <Form.Control name="fileImg" type="file" accept=".jpg, .jpeg, .png" style={{ width: 290 }}
                                            onChange={e => showPreview(e, form)}
                                            isInvalid={!!errors.fileImg} />
                                    )}
                                </Field>
                                <Form.Control.Feedback className="m-0" type="invalid">{errors.img}</Form.Control.Feedback>
                            </Form.Group>
                            <Row className="mt-2">
                                <Form.Group as={Col} sm={8}>
                                    <Form.Label className="mb-0">Имя пользователя (логин)</Form.Label><sup>*</sup>
                                    <Form.Control type="text"
                                        name="userName" value={values.userName ?? ""} onChange={handleChange}
                                        isInvalid={!!errors.userName} />
                                    <Form.Control.Feedback type="invalid">{errors.userName}</Form.Control.Feedback>
                                </Form.Group>
                                <Form.Group as={Col} sm={4}>
                                    <Form.Label className="mb-0">Пароль</Form.Label><sup>*</sup>
                                    <Form.Control type="text"
                                        name="password" value={values.password ?? ""} onChange={handleChange}
                                        isInvalid={!!errors.password} />
                                    <Form.Control.Feedback type="invalid">{errors.password}</Form.Control.Feedback>
                                </Form.Group>
                            </Row>
                            <Row className="mt-2">
                                <Form.Group as={Col} sm={4}>
                                    <Form.Label className="mb-0">Фамилия</Form.Label><sup>*</sup>
                                    <Form.Control type="text"
                                        name="surname" value={values.surname ?? ""} onChange={handleChange}
                                        isInvalid={!!errors.surname} />
                                    <Form.Control.Feedback type="invalid">{errors.surname}</Form.Control.Feedback>
                                </Form.Group>
                                <Form.Group as={Col} sm={4}>
                                    <Form.Label className="mb-0">Имя</Form.Label><sup>*</sup>
                                    <Form.Control type="text"
                                        name="name" value={values.name ?? ""} onChange={handleChange}
                                        isInvalid={!!errors.name} />
                                    <Form.Control.Feedback type="invalid">{errors.name}</Form.Control.Feedback>
                                </Form.Group>
                                <Form.Group as={Col} sm={4}>
                                    <Form.Label className="mb-0">Отчество</Form.Label><sup>*</sup>
                                    <Form.Control type="text"
                                        name="patronymic" value={values.patronymic ?? ""} onChange={handleChange}
                                        isInvalid={!!errors.patronymic} />
                                    <Form.Control.Feedback type="invalid">{errors.patronymic}</Form.Control.Feedback>
                                </Form.Group>
                            </Row>
                        </Modal.Body>
                        <Modal.Footer>
                            <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                            <Button type="submit" variant="success">Сохранить</Button>
                        </Modal.Footer>
                    </Form >
                )}
            </Formik>
        </Modal>
    );
}