import UsersService from "../../../services/Users.service.js";
import { ChangeUserValidationSchema } from '../../../validations/User.validation';

import ModalWindowPreloader from "../../ModalWindowPreloader";

import Button from 'react-bootstrap/Button';
import Col from 'react-bootstrap/Col';
import Form from 'react-bootstrap/Form';
import Image from 'react-bootstrap/Image';
import Modal from 'react-bootstrap/Modal';
import Row from 'react-bootstrap/Row';

import { Field } from 'formik';
import * as formik from 'formik';

import { useDispatch, useSelector } from 'react-redux';
import React, { useState } from 'react';
import { authActions } from '../../../../_store';

export default function ModalWindowProfile({ show, handleClose }) {
    const dispatch = useDispatch();
    const logout = () => dispatch(authActions.logout());
    const authUser = useSelector(x => x.auth.user);
    const { Formik } = formik;
    const [validated, setValidated] = useState(false);
    const [updatedUser, setUpdatedUser] = useState(null);
    const [preview, setPreview] = React.useState("/img/Users/bntu.jpg");

    const handleSubmit = (values) => {
        onEditUser(values);
        setValidated(true);
    }

    const onEditUser = async (updatedUser) => {
        if (authUser.id !== null) {
            const form = new FormData();
            form.append("Id", updatedUser.id);
            form.append("Name", updatedUser.name);
            form.append("Surname", updatedUser.surname);
            form.append("Patronymic", updatedUser.patronymic);
            form.append("FileImg", updatedUser.fileImg);

            await UsersService.httpPut(authUser.id, form);
            handleClose();
            setValidated(false);
        }
    }
    const getUserById = async () => {
        try {
            var userData = await UsersService.httpGetById(authUser.id);
            setUpdatedUser(userData);
            setPreview(userData.img);
        }
        catch {
            logout();
        }
    }
    const onClose = () => {
        setValidated(false);
        handleClose();
    }

    const showPreview = (e, form) => {
        const reader = new FileReader();
        reader.onload = x => {
            setPreview(x.target.result);
        }
        reader.readAsDataURL(e.target.files[0]);
        form.setFieldValue("fileImg", e.currentTarget.files[0]);
    };

    React.useEffect(() => {
        if (authUser) {
            getUserById(authUser.id);
        }
    }, [authUser.id]);

    if (!updatedUser) {
        return <ModalWindowPreloader show={show} handleClose={handleClose} />;
    }
    else {
        return (
            <Modal show={show} onHide={onClose} backdrop="static" keyboard={false}>
                <Formik
                    validationSchema={ChangeUserValidationSchema}
                    onSubmit={handleSubmit}
                    initialValues={{ ...updatedUser, fileImg: null }}>
                    {({ handleSubmit, handleChange, values, touched, errors }) => (
                        <Form noValidate validated={validated} onSubmit={handleSubmit}>
                            <Modal.Header closeButton>
                                <Modal.Title as="h5">Профиль</Modal.Title>
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
                                <Button variant="secondary" onClick={onClose}>Закрыть</Button>
                                <Button type="submit" variant="success">Изменить</Button>
                            </Modal.Footer>
                        </Form >
                    )}
                </Formik>
            </Modal>
        );
    }
}