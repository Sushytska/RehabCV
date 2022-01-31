import React, { useState } from 'react';
import people from './data';
import { FaChevronLeft, FaChevronRight, FaQuoteRight } from 'react-icons/fa';
const Review = () => {
    const [index, setIndex] = useState(0);
    const { name, job, image, text } = people[index];
    const checkNumber = (number) => {
        if (number > people.length - 1) {
            return 0;
        }
        if (number < 0) {
            return people.length - 1;
        }
        return number;
    };
    const nextPerson = () => {
        setIndex((index) => {
            let newIndex = index + 1;
            return checkNumber(newIndex);
        });
    };
    const prevPerson = () => {
        setIndex((index) => {
            let newIndex = index - 1;
            return checkNumber(newIndex);
        });
    };
    const randomPerson = () => {
        let randomNumber = Math.floor(Math.random() * people.length);
        if (randomNumber === index) {
            randomNumber = index + 1;
        }
        setIndex(checkNumber(randomNumber));
    };

    return (
        <article className='review'>
            <div className='img-container'>
                <img src={image} alt={name} className='person-img' />
                <span className='quote-icon'>
                    <FaQuoteRight />
                </span>
            </div>
            <h4 className='author'>{name}</h4>
            <p className='job'>{job}</p>
            <p className='info'>{text}</p>
            <div className='button-container'>
                <button className='prev-btn' onClick={prevPerson}>
                    <FaChevronLeft />
                </button>
                <button className='next-btn' onClick={nextPerson}>
                    <FaChevronRight />
                </button>
            </div>
            <button className='random-btn' onClick={randomPerson}>
                surprise me
            </button>
        </article>
    );
};

export default Review;
import React, { useState } from 'react';
import { AiOutlineMinus, AiOutlinePlus } from 'react-icons/ai';
const Question = ({ title, info }) => {
    const [showInfo, setShowInfo] = useState(false);
    return (
        <article className='question'>
            <header>
                <h4>{title}</h4>
                <button className='btn' onClick={() => setShowInfo(!showInfo)}>
                    {showInfo ? <AiOutlineMinus /> : <AiOutlinePlus />}
                </button>
            </header>
            {showInfo && <p>{info}</p>}
        </article>
    );
};

export default Question;
import React, { useState, useEffect } from 'react'
import { FiChevronRight, FiChevronLeft } from 'react-icons/fi'
import { FaQuoteRight } from 'react-icons/fa'
import data from './data'
function App() {
    const [people, setPeople] = useState(data)
    const [index, setIndex] = React.useState(0)

    const nextSlide = () => {
        setIndex((oldIndex) => {
            let index = oldIndex + 1
            if (index > people.length - 1) {
                index = 0
            }
            return index
        })
    }
    const prevSlide = () => {
        setIndex((oldIndex) => {
            let index = oldIndex - 1
            if (index < 0) {
                index = people.length - 1
            }
            return index
        })
    }

    // useEffect(() => {
    //   const lastIndex = people.length - 1
    //   if (index < 0) {
    //     setIndex(lastIndex)
    //   }
    //   if (index > lastIndex) {
    //     setIndex(0)
    //   }
    // }, [index, people])

    useEffect(() => {
        let slider = setInterval(() => {
            setIndex((oldIndex) => {
                let index = oldIndex + 1
                if (index > people.length - 1) {
                    index = 0
                }
                return index
            })
        }, 5000)
        return () => {
            clearInterval(slider)
        }
    }, [index])

    return (
        <section className='section'>
            <div className='title'>
                <h2>
                    <span>/</span>reviews
                </h2>
            </div>
            <div className='section-center'>
                {people.map((person, personIndex) => {
                    const { id, image, name, title, quote } = person

                    let position = 'nextSlide'
                    if (personIndex === index) {
                        position = 'activeSlide'
                    }
                    if (
                        personIndex === index - 1 ||
                        (index === 0 && personIndex === people.length - 1)
                    ) {
                        position = 'lastSlide'
                    }

                    return (
                        <article className={position} key={id}>
                            <img src={image} alt={name} className='person-img' />
                            <h4>{name}</h4>
                            <p className='title'>{title}</p>
                            <p className='text'>{quote}</p>
                            <FaQuoteRight className='icon' />
                        </article>
                    )
                })}
                <button className='prev' onClick={prevSlide}>
                    <FiChevronLeft />
                </button>
                <button className='next' onClick={nextSlide}>
                    <FiChevronRight />
                </button>
            </div>
        </section>
    )
}

export default App;
import React, { useState, useEffect } from 'react'
import rgbToHex from './utils'

const SingleColor = ({ rgb, weight, index, hexColor }) => {
    const [alert, setAlert] = useState(false)
    const bcg = rgb.join(',')
    const hex = rgbToHex(...rgb)
    const hexValue = `#${hexColor}`
    useEffect(() => {
        const timeout = setTimeout(() => {
            setAlert(false)
        }, 3000)
        return () => clearTimeout(timeout)
    }, [alert])
    return (
        <article
            className={`color ${index > 10 && 'color-light'}`}
            style={{ backgroundColor: `rgb(${bcg})` }}
            onClick={() => {
                setAlert(true)
                navigator.clipboard.writeText(hexValue)
            }}
        >
            <p className='percent-value'>{weight}%</p>
            <p className='color-value'>{hexValue}</p>
            {alert && <p className='alert'>copied to clipboard</p>}
        </article>
    )
}

export default SingleColor
import React from 'react';
import { FaEdit, FaTrash } from 'react-icons/fa';
const List = ({ items, removeItem, editItem }) => {
    return (
        <div className='grocery-list'>
            {items.map((item) => {
                const { id, title } = item;
                return (
                    <article className='grocery-item' key={id}>
                        <p className='title'>{title}</p>
                        <div className='btn-container'>
                            <button
                                type='button'
                                className='edit-btn'
                                onClick={() => editItem(id)}
                            >
                                <FaEdit />
                            </button>
                            <button
                                type='button'
                                className='delete-btn'
                                onClick={() => removeItem(id)}
                            >
                                <FaTrash />
                            </button>
                        </div>
                    </article>
                );
            })}
        </div>
    );
};

export default List;

