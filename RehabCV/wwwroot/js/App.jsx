import React from 'react';
import { FaQuoteRight } from 'react-icons/fa';
import people from './data';

const People = ({ id, image, name, title, quote, personIndex, index }) => {
    let position = "nextSlide";
    if (personIndex === index) {
        position = 'activeSlide'
    }
    if (personIndex === index - 1 || (index === 0 && personIndex === people.length - 1)) {
        position = 'lastSlide'
    }
    return (
        <article className={position} key={id}>
            <img src={image} alt={name} className="person-img" />
            <h4>{name}</h4>
            <p className="title">{title}</p>
            <p className="text">{quote}</p>
            <FaQuoteRight className="icon" />
        </article>
    );
}

export default People;


export function getHashValues(hash) {
    return Object.values(hash) // needs modern browser
}

export function hashById(array) {
    let hash = {}

    for (let item of array) {
        hash[item.id] = item
    }

    return hash
}

export function excludeById(array, id) {
    return array.filter((item) => item.id !== id)
}

export function getTodayStr() {
    return new Date().toISOString().replace(/T.*$/, '')
}
