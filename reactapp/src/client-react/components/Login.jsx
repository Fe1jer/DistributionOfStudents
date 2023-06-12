import Button from 'react-bootstrap/Button';
import Card from 'react-bootstrap/Card';
import Col from 'react-bootstrap/Col';
import Form from 'react-bootstrap/Form';

import { yupResolver } from '@hookform/resolvers/yup';
import { useEffect } from 'react';
import { useForm } from "react-hook-form";
import { useDispatch, useSelector } from 'react-redux';
import * as Yup from 'yup';

import { history } from '../../_helpers';
import { authActions } from '../../_store';


export default function Login() {
    const dispatch = useDispatch();
    const authUser = useSelector(x => x.auth.user);
    const authError = useSelector(x => x.auth.error);

    useEffect(() => {
        // redirect to home if already logged in
        if (authUser) history.navigate('/');
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    // form validation rules 
    const validationSchema = Yup.object().shape({
        username: Yup.string().required('Введите имя пользователя'),
        password: Yup.string().required('Введите пароль').min(6, "Длина пароля не менее 6 символов")
    });
    const formOptions = { resolver: yupResolver(validationSchema) };

    // get functions to build form with useForm() hook
    const { register, handleSubmit, formState } = useForm(formOptions);
    const { errors, isSubmitting } = formState;

    function onSubmit({ username, password }) {
        return dispatch(authActions.login({ username, password }));
    }

    return (
        <Col md={{ span: 4, offset: 4 }} className="pt-5">
            <Card>
                <Card.Header><h4 className="d-inline">Вход</h4> (только для приёмной комиссии)</Card.Header>
                <Card.Body>
                    <Form onSubmit={handleSubmit(onSubmit)}>
                        <Form.Group>
                            <Form.Label>Имя пользователя</Form.Label><sup>*</sup>
                            <Form.Control type="text" name="username" {...register('username')}
                                isInvalid={errors.username ? !!errors.username : false} />
                            <Form.Control.Feedback type="invalid">{errors.username?.message}</Form.Control.Feedback>
                        </Form.Group>
                        <Form.Group className="mt-3">
                            <Form.Label>Пароль</Form.Label><sup>*</sup>
                            <Form.Control type="password" name="password" {...register('password')}
                                isInvalid={errors.password ? !!errors.password : false} />
                            <Form.Control.Feedback type="invalid">{errors.password?.message}</Form.Control.Feedback>
                        </Form.Group>
                        <Button disabled={isSubmitting} type="" variant="success" className="mt-3">
                            {isSubmitting && <span className="spinner-border spinner-border-sm mr-1"></span>}
                            Войти
                        </Button>
                        {authError &&
                            <div className="alert alert-danger mt-3 mb-0">{authError.message}</div>
                        }
                    </Form>
                </Card.Body>
            </Card>
        </Col>
    )
}
