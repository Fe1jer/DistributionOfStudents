import * as yup from 'yup';

export const CreateUserValidationSchema = yup.object().shape({
    userName: yup.string().required('Введите имя'),
    password: yup.string().required('Введите имя').min(6, "Не менее 6 символов"),
    name: yup.string().required('Введите имя'),
    surname: yup.string().required('Введите фамилию'),
    patronymic: yup.string().required("Введите отчество"),
    fileImg: yup.mixed().nullable(true),
});

export const ChangeUserValidationSchema = yup.object().shape({
    id: yup.string().matches(/^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/i),
    name: yup.string().required('Введите имя'),
    surname: yup.string().required('Введите фамилию'),
    patronymic: yup.string().required("Введите отчество"),
    fileImg: yup.mixed().nullable(true),
});